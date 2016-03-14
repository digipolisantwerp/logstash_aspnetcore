using System;
using Microsoft.Extensions.Logging;
using Toolbox.Logstash.Message;
using Toolbox.Logstash.Options.Internal;

namespace Toolbox.Logstash.Loggers
{
    public class LogstashLogger : ILogger
    {
        public LogstashLogger(string name, ILogMessageBuilder logMessageBuilder, ILogstashHttpLogger logger)
        {
            if ( logMessageBuilder == null ) throw new ArgumentNullException(nameof(logMessageBuilder), $"{nameof(logMessageBuilder)} cannot be null.");
            if ( logger == null ) throw new ArgumentNullException(nameof(logger), $"{nameof(logger)} cannot be null.");
            LogMessageBuilder = logMessageBuilder;
            Logger = logger;
            Name = String.IsNullOrWhiteSpace(name) ? Defaults.HttpLogger.Name : name;
        }

        internal string Name { get; private set; }
        internal ILogstashHttpLogger Logger { get; private set; }
        internal ILogMessageBuilder LogMessageBuilder { get; private set; }

        public IDisposable BeginScopeImpl(object state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            // ToDo (SVB) : do we need to implement this ?
            return true;
        }

        public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
           var logMessage = LogMessageBuilder.Build(Name, logLevel, state, exception, formatter);
           Logger.Log(logMessage);
        }
    }
}
