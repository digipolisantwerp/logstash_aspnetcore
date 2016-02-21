using System;
#if NET451 || DNX451
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
#else
using System.Threading;
#endif
using Microsoft.Extensions.Logging;
using Toolbox.Logstash.Message;
using FrameworkLogger = Microsoft.Extensions.Logging.ILogger;

namespace Toolbox.Logstash
{
    public class LogstashHttpLoggerProvider : ILoggerProvider
    {
        private readonly Guid _logId;
        private Client.ILogger _logger;

        public LogstashHttpLoggerProvider(Guid logId, Client.ILogger logger = null)
        {
            _logId = logId;
            _logger = logger;
        }

        public ILogger CreateLogger(string name)
        {
            if (_logger == null) _logger = Client.Logger.Create(_logId);
            return new Logger(this, _logger);
        }

        public void Dispose()
        { 
}

        private class Logger : ILogger
        {
            private Client.ILogger _logger;
            private LogstashHttpLoggerProvider _provider;

            public Logger(LogstashHttpLoggerProvider provider, Client.ILogger logger)
            {
                _provider = provider;
                _logger = logger;
            }

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
                if (string.IsNullOrWhiteSpace(state.ToString())) return;

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
}
