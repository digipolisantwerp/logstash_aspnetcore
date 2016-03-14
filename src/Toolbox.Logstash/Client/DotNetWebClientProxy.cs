using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mannex.Net;
using Mannex.Net.Mime;
using Mannex.Threading.Tasks;

namespace Toolbox.Logstash.Client
{
    internal class DotNetWebClientProxy : IWebClient
    {
        public DotNetWebClientProxy(string uriString, string userAgent)
        {
            if ( String.IsNullOrWhiteSpace(userAgent) ) throw new ArgumentException($"{nameof(userAgent)} is mandatory.", nameof(userAgent));
                Uri = new Uri(uriString);
        }

        public Uri Uri { get; private set; }            // http://e27-elk.cloudapp.net:8080/api/v2/messages

        private string _userAgent;
        private string UserAgent
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_userAgent) )          // ToDo (SVB) : user agent injecteren
                {
                    try
                    {
                        _userAgent = string.Format("digipolis.be logstash client/{0}", Assembly.GetExecutingAssembly().GetName().Version);
                    }
                    catch
                    {
                    }
                }

                return _userAgent;
            }
        }

        public Task<T> Post<T>(string data, WebHeaderCollection headers, Func<WebHeaderCollection, string, T> resultor)         // data as first param
        {
            if (data == null) throw new ArgumentNullException("data");
            if (resultor == null) throw new ArgumentNullException("resultor");

            var request = (HttpWebRequest)WebRequest.Create(Uri);
            request.Method = "POST";
            request.Timeout = 5000;
            request.UserAgent = UserAgent;

            if (headers != null)
            {
                // Some headers like Content-Type cannot be added directly
                // and therefore must be treated specially and go over 
                // the corresponding property on the HttpWebRequest object.

                headers = new WebHeaderCollection { headers };
                var contentType = headers[HttpRequestHeader.ContentType];
                if (contentType != null)
                {
                    request.ContentType = contentType;
                    headers.Remove(HttpRequestHeader.ContentType);
                }

                request.Headers.Add(headers);
            }

            var encoding = Encoding.UTF8;
            var bytes = encoding.GetBytes(data);
            request.ContentLength = bytes.Length;

            return Spawn<T>(tcs => Post(request, bytes, tcs, resultor));
        }

        static IEnumerable<Task> Post<T>(WebRequest request, byte[] bytes, TaskCompletionSource<T> tcs, Func<WebHeaderCollection, string, T> resultor)
        {
            Debug.Assert(request != null);
            Debug.Assert(bytes != null);
            Debug.Assert(tcs != null);
            Debug.Assert(resultor != null);

            var getRequestStreamTask = request.GetRequestStreamAsync();
            yield return getRequestStreamTask;

            using (var stream = getRequestStreamTask.Result)
            {
                var writeTask = stream.WriteAsync(bytes, 0, bytes.Length);
                yield return writeTask;
            }

            foreach (var task in GetResponse(request, tcs, resultor))
                yield return task;
        }

        public Task<T> Get<T>(string id, WebHeaderCollection headers, Func<WebHeaderCollection, string, T> resultor)
        {
            if (resultor == null) throw new ArgumentNullException("resultor");

            var request = (HttpWebRequest)WebRequest.Create(new Uri(Uri, id));
            request.UserAgent = UserAgent;

            if (headers != null)
                request.Headers.Add(headers);

            return Spawn<T>(tcs => GetResponse(request, tcs, resultor));
        }

        static IEnumerable<Task> GetResponse<T>(WebRequest request, TaskCompletionSource<T> tcs, Func<WebHeaderCollection, string, T> resultor)
        {
            Debug.Assert(request != null);
            Debug.Assert(tcs != null);
            Debug.Assert(resultor != null);

            var getResponseTask = request.GetResponseAsync();
            yield return getResponseTask;

            using (var response = getResponseTask.Result)
            using (var stream = response.GetResponseStream())
            {
                var contentType = response.Headers.Map(HttpResponseHeader.ContentType, h => new ContentType(h));
                var encoding = contentType != null
                             ? contentType.EncodingFromCharSet(Encoding.Default)
                             : Encoding.Default;

                var sb = new StringBuilder();
                foreach (var task in ReadAllText(stream, encoding, sb))
                    yield return task;

                tcs.SetResult(resultor(response.Headers, sb.ToString()));
            }
        }

        static IEnumerable<Task> ReadAllText(Stream stream, Encoding encoding, StringBuilder output)
        {
            Debug.Assert(stream != null);
            Debug.Assert(encoding != null);
            Debug.Assert(output != null);

            var bytes = new byte[4096];
            var chars = (char[])null;
            var decoder = encoding.GetDecoder();
            while (true)
            {
                var readTask = stream.ReadAsync(bytes, 0, bytes.Length);
                yield return readTask;
                var readCount = readTask.Result;
                if (readCount == 0)
                    break;
                var charCount = decoder.GetCharCount(bytes, 0, readCount);
                if (chars == null || charCount > chars.Length)
                    chars = new char[charCount];
                var decodedCharCount = decoder.GetChars(bytes, 0, readCount, chars, 0, false);
                output.Append(chars, 0, decodedCharCount);
            }
        }

        static Task<T> Spawn<T>(Func<TaskCompletionSource<T>, IEnumerable<Task>> jobFunc)
        {
            Debug.Assert(jobFunc != null);

            var tcs = new TaskCompletionSource<T>();

            Task.Factory
                .StartNew(jobFunc(tcs))
                .ContinueWith(t =>
                {
                    if (t.IsCanceled)
                    {
                        tcs.TrySetCanceled();
                    }
                    else if (t.IsFaulted)
                    {
                        var aggregate = t.Exception;
                        Debug.Assert(aggregate != null);
                        tcs.TrySetException(aggregate.InnerExceptions);
                    }
                    else
                    {
                        Debug.Assert(t.Status == TaskStatus.RanToCompletion);
                    }
                },
                TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }
    }
}
