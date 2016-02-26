using System;
#if NET451 || DNX451
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
#else
using System.Threading;
#endif
using Microsoft.Extensions.Logging;
using Toolbox.Logstash.Loggers;

namespace Toolbox.Logstash
{
    public class LogstashHttpLoggerProvider : ILoggerProvider
    {
        public LogstashHttpLoggerProvider(ILogstashLogger logger = null)
        {
            _logger = logger;
        }

        private object _synclock = new object();

        public ILogger CreateLogger(string name)
        {
            if ( _logger == null )
            {
                lock ( _synclock )
                {
                    if ( _logger == null ) _logger = new LogstashHttpLogger();
                }
            }
            return new LogstashLogger(_logger);
        }

        private ILogstashLogger _logger;

        public void Dispose()
        { }
    }
}
