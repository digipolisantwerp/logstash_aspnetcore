using System;
using Microsoft.Extensions.Logging;
using Toolbox.Logstash.Client;
using Toolbox.Logstash.Loggers;
using Toolbox.Logstash.Message;

namespace Toolbox.Logstash
{
    public class LogstashHttpLoggerProvider : ILoggerProvider
    {
        public LogstashHttpLoggerProvider(IServiceProvider serviceProvider, LogstashOptions options)
        {
            if ( serviceProvider == null ) throw new ArgumentNullException(nameof(serviceProvider), $"{nameof(serviceProvider)} cannot be null.");
            if ( options == null ) throw new ArgumentNullException(nameof(options), $"{nameof(options)} cannot be null.");
            Options = options;
            ServiceProvider = serviceProvider;
        }

        internal IServiceProvider ServiceProvider { get; private set; }
        internal LogstashOptions Options { get; private set; }
        
        public ILogger CreateLogger(string name)
        {
            var builder = new LogMessageBuilder();
            var webClient = new DotNetWebClientProxy();     // ToDo (SVB) : nodige options hier injecten
            var logger = new LogstashHttpLogger(webClient);
            return new LogstashLogger(ServiceProvider, builder, Options, logger);
        }

        public void Dispose()
        { }
    }
}
