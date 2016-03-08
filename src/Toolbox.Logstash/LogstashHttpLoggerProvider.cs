using System;
using Microsoft.Extensions.Logging;
using Toolbox.Logstash.Client;
using Toolbox.Logstash.Loggers;

namespace Toolbox.Logstash
{
    public class LogstashHttpLoggerProvider : ILoggerProvider
    {
        public LogstashHttpLoggerProvider(ILogstashHttpLogger logger)
        {
            if ( options == null ) throw new ArgumentNullException(nameof(options), $"{nameof(options)} cannot be null.");

            Options = options;
            Logger = logger;
        }

        private object _synclock = new object();

        internal LogstashOptions Options { get; private set; }
        internal ILogstashHttpLogger Logger { get; private set; }
        
        public ILogger CreateLogger(string name)
        {
            return new LogstashLogger(Logger);
        }

        public void Dispose()
        { }
    }
}
