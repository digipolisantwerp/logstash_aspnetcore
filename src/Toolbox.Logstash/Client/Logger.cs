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

namespace Toolbox.Logstash.Client
{
    /// <summary>
    /// Implementation of the ILogger interface for logging messages to the logstash API.
    /// </summary>
    public class Logger : ILogger
    {
        private readonly Guid _logId;
        private readonly Uri _url = new Uri("http://e27-elk.cloudapp.net:8080/");
        private readonly IWebClient _webClient;
        private string _localIPAddress;
        private string _currentProcessID;

        public Guid LogId { get { return _logId; } }
        public Uri Url { get { return _url; } }

        public event EventHandler<MessageEventArgs> OnMessage;

        public event EventHandler<FailEventArgs> OnMessageFail;

        /// <summary>
        /// Creates a new logger with the specified log ID. The logger is configured to the test setup on Azure.
        /// This is probably the constructor you want to use 99 % of the times.
        /// </summary>
        public Logger(Guid logId) : this(logId, null)
        {
        }

        /// <summary>
        /// Creates a new logger with the specified log ID and URL. 
        /// </summary>
        public Logger(Guid logId, Uri url) : this(logId, url, new DotNetWebClientProxy())
        {
        }

        internal Logger(Guid logId, Uri url, IWebClient webClient)
        {
            _logId = logId;
            if (url != null) _url = url;
            _webClient = webClient;
            _localIPAddress = GetLocalIPAddress();
            _currentProcessID = GetCurrentProcessID();
        }

        /// <summary>
        /// Creates a instance of the logger. Do exactly the same as calling the constructor with a Guid parameter.
        /// </summary>
        public static Logger Create(Guid logId)
        {
            return new Logger(logId);
        }

        public void Verbose(string messageTemplate, params object[] propertyValues)
        {
            Verbose(null, messageTemplate, propertyValues);
        }

        public void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Log(exception, LogStashLevel.Trace, messageTemplate, propertyValues);
        }

        public void Debug(string messageTemplate, params object[] propertyValues)
        {
            Debug(null, messageTemplate, propertyValues);
        }

        public void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Log(exception, LogStashLevel.Debug, messageTemplate, propertyValues);
        }

        public void Information(string messageTemplate, params object[] propertyValues)
        {
            Information(null, messageTemplate, propertyValues);
        }

        public void Information(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Log(exception, LogStashLevel.Information, messageTemplate, propertyValues);
        }

        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            Warning(null, messageTemplate, propertyValues);
        }

        public void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Log(exception, LogStashLevel.Warning, messageTemplate, propertyValues);
        }

        public void Error(string messageTemplate, params object[] propertyValues)
        {
            Error(null, messageTemplate, propertyValues);
        }

        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Log(exception, LogStashLevel.Error, messageTemplate, propertyValues);
        }

        public void Critical(string messageTemplate, params object[] propertyValues)
        {
            Critical(null, messageTemplate, propertyValues);
        }

        public void Critical(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Log(exception, LogStashLevel.Critical, messageTemplate, propertyValues);
        }

        void Log(Exception exception, LogStashLevel level, string messageTemplate, params object[] propertyValues)
        {
            LogMessage message = new LogMessage();
            //TBD enrich message

            Log(message);
        }

        public string Log(LogMessage message)
        {
            message.Header.IPAddress = _localIPAddress;
            message.Header.ProcessId = _currentProcessID;
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

            return _webClient.Post(headers, ApiUrl(), json)
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
            return _webClient.Get(ApiUrl(new NameValueCollection { { "id", id } }))
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

        public string GetCurrentProcessID()
        {
            return Process.GetCurrentProcess().Id.ToString();
        }

        private Uri ApiUrl(NameValueCollection query = null)
        {
            return new Uri(_url, "api/v2/messages");
        }
    }
}
