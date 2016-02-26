using System;
using Toolbox.Logstash.Client;
using Toolbox.Logstash.Message;

namespace Toolbox.Logstash.Loggers
{
    /// <summary>
    /// The raw logger logging messages to logstash.
    /// </summary>
    public interface ILogstashLogger
    {
        /// <summary>
        /// By subscribing to the OnMessage event, you can hook into the pipeline of logging a message to logstash.
        /// The event is triggered just before calling the logstash API.
        /// </summary>
        event EventHandler<MessageEventArgs> OnMessage;

        /// <summary>
        /// By subscribing to the OnMessageFail event, you can get a call if an error happened while logging a message
        /// to logstash. In this case you should do something to log the message elsewhere.
        /// </summary>
        event EventHandler<FailEventArgs> OnMessageFail;

        /// <summary>
        /// Write a log message using the specified Message. The message encapsulates the data included in properties.
        /// </summary>
        string Log(LogMessage message);

        /// <summary>
        /// Async write a log message using the specified Message. The message encapsulates the data included in properties.
        /// </summary>
        IAsyncResult BeginLog(LogMessage message, AsyncCallback asyncCallback, object asyncState);

        /// <summary>
        /// Async end of writing a log message.
        /// </summary>
        string EndLog(IAsyncResult asyncResult);

        /// <summary>
        /// Gets a log message by its ID.
        /// </summary>
        LogMessage GetMessage(string id);

        /// <summary>
        /// Async gets a log message by its ID.
        /// </summary>
        IAsyncResult BeginGetMessage(string id, AsyncCallback asyncCallback, object asyncState);

        /// <summary>
        /// Async end of get log message.
        /// </summary>
        LogMessage EndGetMessage(IAsyncResult asyncResult);

        /// <summary>
        /// Get all messages in the specified page and in the page size.
        /// </summary>
        MessagesResult GetMessages(int pageIndex, int pageSize);

        /// <summary>
        /// Async get all messages in the specified page and in the page size.
        /// </summary>
        IAsyncResult BeginGetMessages(int pageIndex, int pageSize, AsyncCallback asyncCallback, object asyncState);

        /// <summary>
        /// Async end of getting all messages.
        /// </summary>
        MessagesResult EndGetMessages(IAsyncResult asyncResult);
    }
}
