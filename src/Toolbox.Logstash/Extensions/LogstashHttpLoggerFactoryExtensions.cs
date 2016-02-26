using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Configuration;

namespace Toolbox.Logstash
{
    public static class LogstashHttpLoggerFactoryExtensions
    {
        public static ILoggerFactory AddLogstashHttp(this ILoggerFactory factory, Action<LogstashOptions> setupAction)
        {
            if ( setupAction == null ) throw new ArgumentNullException(nameof(setupAction), $"{nameof(setupAction)} cannot be null.");

                // ToDo (SVB) : register options

                factory.AddProvider(new LogstashHttpLoggerProvider());
            return factory;
        }

        public static ILoggerFactory AddLogstashHttp(this ILoggerFactory factory, IConfiguration config)
        {
            if ( config == null ) throw new ArgumentNullException(nameof(config), $"{nameof(config)} cannot be null.");

            // ToDo (SVB) : register optionss

            factory.AddProvider(new LogstashHttpLoggerProvider());
            return factory;
        }
    }
}
