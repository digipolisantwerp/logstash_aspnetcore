using System;
using Microsoft.Extensions.Logging;
using Toolbox.Logstash.Message;

namespace Toolbox.Logstash.Loggers
{
    public class LogstashLogger : ILogger
    {
        public LogstashLogger(ILogstashLogger logger)
        {
            _logger = logger;
        }

        private ILogstashLogger _logger;

        public IDisposable BeginScopeImpl(object state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            // The logstash client doesn't implement the concept of enabled/disabled log levels or a minimum level.
            return true;
        }

        public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            if ( state == null ) return;
            if ( string.IsNullOrWhiteSpace(state.ToString()) ) return;

            var logstashLevel = LogLevelToLogStashLevel(logLevel);
            var message = new LogMessage(logstashLevel);
            // Todo:  initialize the logmessage

            _logger.Log(message);
        }

        private LogStashLevel LogLevelToLogStashLevel(LogLevel logLevel)
        {
            var level = LogStashLevel.Information;

            switch ( logLevel )
            {
                case LogLevel.Debug:
                    level = LogStashLevel.Debug;
                    break;
                case LogLevel.Verbose:
                    level = LogStashLevel.Trace;
                    break;
                case LogLevel.Information:
                    level = LogStashLevel.Information;
                    break;
                case LogLevel.Warning:
                    level = LogStashLevel.Warning;
                    break;
                case LogLevel.Error:
                    level = LogStashLevel.Error;
                    break;
                case LogLevel.Critical:
                    level = LogStashLevel.Critical;
                    break;
                case LogLevel.None:
                    level = LogStashLevel.None;
                    break;
            }

            return level;
        }
    }
}
