using System;
#if NET451 || DNX451
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
#else
using System.Threading;
#endif
using Microsoft.Extensions.Logging;
using FrameworkLogger = Microsoft.Extensions.Logging.ILogger;

namespace Toolbox.Logstash
{
    public class LogstashHTTPInputLoggerProvider : ILoggerProvider//, ILogEventEnricher
    {
        private readonly Guid _logId;
        private Client.ILogger _logger;

        public LogstashHTTPInputLoggerProvider(Guid logId, Client.ILogger logger = null)
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
            private LogstashHTTPInputLoggerProvider _provider;

            public Logger(LogstashHTTPInputLoggerProvider provider, Client.ILogger logger)
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
                if (state == null) throw new ArgumentNullException("state");
                if (string.IsNullOrWhiteSpace(state.ToString())) throw new ArgumentException("state");

                //var message = new Client.LogMessage();
                //TBD initialize the logmessage
                Client.LogMessage message = state as Client.LogMessage;
                if (message != null)
                {
                    _logger.Log(message);
                }
            }

            private Client.LogLevel? LogLevelToSeverity(LogLevel logLevel)
            {
                if (logLevel == LogLevel.Debug)
                {
                    return Client.LogLevel.DEBUG;
                }
                else if (logLevel == LogLevel.Verbose)
                {
                    return Client.LogLevel.VERBOSE;
                }
                else if (logLevel == LogLevel.Warning)
                {
                    return Client.LogLevel.WARNING;
                }
                else if (logLevel == LogLevel.Error)
                {
                    return Client.LogLevel.ERROR;
                }
                else if (logLevel == LogLevel.Critical)
                {
                    return Client.LogLevel.CRITICAL;
                }
                else
                {
                    return Client.LogLevel.INFO;
                }
            }
        }
    }
}
