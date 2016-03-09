using System;

namespace Toolbox.Logstash.Message
{
    public class LogMessageBuilder : ILogMessageBuilder
    {
        public LogMessageBuilder(IServiceProvider serviceProvider, LogstashOptions options)
        {
            if ( serviceProvider == null ) throw new ArgumentNullException(nameof(serviceProvider), $"{nameof(serviceProvider)} cannot be null.");
            if ( options == null ) throw new ArgumentNullException(nameof(options), $"{nameof(options)} cannot be null.");
            ServiceProvider = serviceProvider;
            Options = options;
        }

        internal IServiceProvider ServiceProvider { get; private set; }
        internal LogstashOptions Options { get; private set; }
    }
}
