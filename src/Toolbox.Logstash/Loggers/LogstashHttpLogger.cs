using Mannex.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Toolbox.Logstash.Message;
using Toolbox.Logstash.Client;

namespace Toolbox.Logstash.Loggers
{
    public class LogstashHttpLogger : ILogstashHttpLogger
    {
        public LogstashHttpLogger(IWebClient webClient)
        {
            if ( webClient == null ) throw new ArgumentNullException(nameof(webClient), $"{nameof(webClient)} cannot be null.");
            WebClient = webClient;
        }

        internal IWebClient WebClient { get; private set; }

        public event EventHandler<MessageEventArgs> OnMessage;

        public event EventHandler<FailEventArgs> OnMessageFail;

        public string Log(LogMessage message)
        {
            return EndLog(BeginLog(message, null, null));
        }

        public IAsyncResult BeginLog(LogMessage message, AsyncCallback asyncCallback, object asyncState)
        {
            if (OnMessage != null) OnMessage(this, new MessageEventArgs(message));

            var headers = new WebHeaderCollection { { HttpRequestHeader.ContentType, "application/json" } };

            //TBD using defaults
            //var jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            //jsonSerializerSettings.Converters.Add(new StringEnumConverter());
            //var json = JsonConvert.SerializeObject(message, jsonSerializerSettings);
            var json = JsonConvert.SerializeObject(message);

            WebHeaderCollection nullHeaders = null;
            return WebClient.Post<string>(json, nullHeaders, (responseHeaders, r) => responseHeaders["Location"])           // ToDo (SVB) : resultor func checken
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
            return WebClient.Get<string>(id, null, null)
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
    }
}
