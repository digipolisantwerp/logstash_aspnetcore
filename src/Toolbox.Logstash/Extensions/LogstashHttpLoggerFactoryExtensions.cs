using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Toolbox.Logstash.Options;

namespace Toolbox.Logstash
{
    public static class LogstashHttpLoggerFactoryExtensions
    {
        /// <summary>
        /// Adds the Logstash HTTP Provider to the ASP.NET logging framework.
        /// </summary>
        /// <param name="setupAction">The Logstash HTTP Provider's options.</param>
        /// <returns>The LoggerFactory.</returns>
        public static ILoggerFactory AddLogstashHttp(this ILoggerFactory factory, Action<LogstashOptions> setupAction)
        {
            if ( setupAction == null ) throw new ArgumentNullException(nameof(setupAction), $"{nameof(setupAction)} cannot be null.");

            var options = LogstashOptionsReader.Read(setupAction);
            var provider = new LogstashHttpLoggerProvider(options);

            factory.AddProvider(provider);
            return factory;
        }

        /// <summary>
        /// Adds the Logstash HTTP Provider to the ASP.NET logging framework.
        /// </summary>
        /// <param name="config">An IConfiguration instane that contains the Logstash HTTP provider's options.</param>
        /// <returns>The LoggerFactory.</returns>
        public static ILoggerFactory AddLogstashHttp(this ILoggerFactory factory, IConfiguration config)
        {
            if ( config == null ) throw new ArgumentNullException(nameof(config), $"{nameof(config)} cannot be null.");

            var options = LogstashOptionsReader.Read(config);
            var provider = new LogstashHttpLoggerProvider(options);

            factory.AddProvider(provider);
            return factory;
        }
    }
}
