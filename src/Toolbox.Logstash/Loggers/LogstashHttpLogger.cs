using Mannex.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Toolbox.Logstash.Message;
using Toolbox.Logstash.Client;

namespace Toolbox.Logstash.Loggers
{
    public class LogstashHttpLogger : ILogstashLogger
    {
        public LogstashHttpLogger(LogstashOptions options, IWebClient webClient)
        {
            if ( options == null ) throw new ArgumentNullException(nameof(options), $"{nameof(options)} cannot be null.");
            if ( webClient == null ) throw new ArgumentNullException(nameof(webClient), $"{nameof(webClient)} cannot be null.");

            Options = options;
            WebClient = webClient;
            _localIPAddress = GetLocalIPAddress();
            _currentProcessId = GetCurrentProcessId();
        }

        private readonly Uri _url = new Uri("http://e27-elk.cloudapp.net:8080/");

        public LogstashOptions Options { get; private set; }
        public IWebClient WebClient { get; private set; }

        private string _localIPAddress;
        private string _currentProcessId;

        public Uri Url { get { return _url; } }

        public event EventHandler<MessageEventArgs> OnMessage;

        public event EventHandler<FailEventArgs> OnMessageFail;

        void Log(Exception exception, LogStashLevel level, string messageTemplate, params object[] propertyValues)
        {
            LogMessage message = new LogMessage();
            //TBD enrich message

            Log(message);
        }

        public string Log(LogMessage message)
        {
            message.Header.IPAddress = _localIPAddress;
            message.Header.ProcessId = _currentProcessId;
            message.Header.ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();
            return EndLog(BeginLog(message, null, null));
        }

        public IAsyncResult BeginLog(LogMessage message, AsyncCallback asyncCallback, object asyncState)
        {
            //if (message.DateTime == DateTime.MinValue) message.DateTime = DateTime.UtcNow;
            if (OnMessage != null) OnMessage(this, new MessageEventArgs(message));

            var headers = new WebHeaderCollection { { HttpRequestHeader.ContentType, "application/json" } };

            //TBD using defaults
            //var jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            //jsonSerializerSettings.Converters.Add(new StringEnumConverter());
            //var json = JsonConvert.SerializeObject(message, jsonSerializerSettings);
            var json = JsonConvert.SerializeObject(message);

            return WebClient.Post(headers, ApiUrl(), json)
                             .ContinueWith(t =>
                             {
                                 if (t.Status != TaskStatus.RanToCompletion)
                                 {
                                     if (OnMessageFail != null) OnMessageFail(this, new FailEventArgs(message, t.Exception));
                                     return null;
                                 }

                                 Uri location;
                                 if (!Uri.TryCreate(t.Result, UriKind.Absolute, out location))
                                 {
                                     return null;
                                 }

                                 return (location.Query.TrimStart('?').Split('&').Select(parameter => parameter.Split('='))
                                     .Where(parameterSplitted => parameterSplitted.Length == 2 && parameterSplitted[0] == "id")
                                     .Select(parameterSplitted => parameterSplitted[1]))
                                     .FirstOrDefault();
                             })
                             .Apmize(asyncCallback, asyncState);
        }

        public string EndLog(IAsyncResult asyncResult)
        {
            return EndImpl<string>(asyncResult);
        }

        public LogMessage GetMessage(string id)
        {
            return EndGetMessage(BeginGetMessage(id, null, null));
        }

        public IAsyncResult BeginGetMessage(string id, AsyncCallback asyncCallback, object asyncState)
        {
            return WebClient.Get(ApiUrl(new NameValueCollection { { "id", id } }))
                             .ContinueWith(t =>
                             {
                                 if (t.Status != TaskStatus.RanToCompletion)
                                 {
                                     return null;
                                 }

                                 var message = JsonConvert.DeserializeObject<LogMessage>(t.Result);
                                 return message;
                             })
                             .Apmize(asyncCallback, asyncState);
        }

        public LogMessage EndGetMessage(IAsyncResult asyncResult)
        {
            return EndImpl<LogMessage>(asyncResult);
        }

        public MessagesResult GetMessages(int pageIndex, int pageSize)
        {
            return EndGetMessages(BeginGetMessages(pageIndex, pageSize, null, null));
        }

        public IAsyncResult BeginGetMessages(int pageIndex, int pageSize, AsyncCallback asyncCallback, object asyncState)
        {
            throw new NotImplementedException();
            //var url = ApiUrl(new NameValueCollection
            //{
            //    { "pageindex", pageIndex.ToInvariantString() },
            //    { "pagesize", pageSize.ToInvariantString() },
            //});

            //return _webClient.Get(url).ContinueWith(t =>
            //{
            //    if (t.Status != TaskStatus.RanToCompletion)
            //    {
            //        return null;
            //    }

            //    var messagesResult = JsonConvert.DeserializeObject<MessagesResult>(t.Result);

            //    return messagesResult;
            //}).Apmize(asyncCallback, asyncState);
        }

        public MessagesResult EndGetMessages(IAsyncResult asyncResult)
        {
            return EndImpl<MessagesResult>(asyncResult);
        }

        private T EndImpl<T>(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
            {
                throw new ArgumentNullException(nameof(asyncResult));
            }

            var task = asyncResult as Task<T>;
            if (task == null)
            {
                throw new ArgumentException(null, nameof(asyncResult));
            }

            return task.Result;
        }

        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        public string GetCurrentProcessId()
        {
            return Process.GetCurrentProcess().Id.ToString();
        }

        private Uri ApiUrl(NameValueCollection query = null)
        {
            return new Uri(_url, "api/v2/messages");
        }
    }
}
