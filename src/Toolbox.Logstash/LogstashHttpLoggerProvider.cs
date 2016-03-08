using System;
using Microsoft.Extensions.Logging;
using Toolbox.Logstash.Client;
using Toolbox.Logstash.Loggers;

namespace Toolbox.Logstash
{
    public class LogstashHttpLoggerProvider : ILoggerProvider
    {
        public LogstashHttpLoggerProvider(LogstashOptions options, ILogstashHttpLogger logger = null)
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
            if ( Logger == null )
            {
                lock ( _synclock )
                {
                    if ( Logger == null )
                    {
                        var webClient = new DotNetWebClientProxy();
                        Logger = new LogstashHttpLogger(Options, webClient);
                    }
                }
            }
            return new LogstashLogger(Options, Logger);
        }

        public void Dispose()
        { }
    }
}
