using System;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
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
            //var provider = new LogstashHttpLoggerProvider(options);

            factory.AddProvider(provider);
            return factory;
        }

        /// <summary>
        /// Adds the Logstash HTTP Provider to the ASP.NET logging framework.
        /// </summary>
        /// <param name="config">An IConfiguration instance that contains the Logstash HTTP provider's options.</param>
        /// <returns>The LoggerFactory.</returns>
        public static ILoggerFactory AddLogstashHttp(this ILoggerFactory factory, IConfiguration config)
        {
            if ( config == null ) throw new ArgumentNullException(nameof(config), $"{nameof(config)} cannot be null.");

            var options = LogstashOptionsReader.Read(config);
            //var provider = new LogstashHttpLoggerProvider(options);

            factory.AddProvider(provider);
            return factory;
        }

        /// <summary>
        /// Adds the Logstash HTTP Provider to the ASP.NET logging framework.
        /// </summary>
        /// <param name="app">The IApplicationBuilder.</param>
        /// <returns>The LoggerFactory.</returns>
        public static ILoggerFactory AddLogstashHttp(this ILoggerFactory factory, IApplicationBuilder app)
        {
            if ( app == null ) throw new ArgumentNullException(nameof(app), $"{nameof(app)} cannot be null.");

            var provider = app.ApplicationServices.GetService(typeof(LogstashHttpLoggerProvider)) as LogstashHttpLoggerProvider;
            //if ( provider == null ) throw new ArgumentNullException("LogstashHttpLoggerProvider", "LogstashHttpLoggerProvider is not registered. Are you sure you called services.AddLogstashLogging in the Startup.ConfigureServices method.");

            factory.AddProvider(provider);
            return factory;
        }
    }
}
