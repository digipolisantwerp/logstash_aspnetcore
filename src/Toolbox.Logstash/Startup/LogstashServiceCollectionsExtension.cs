using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Toolbox.Logstash.Client;

namespace Toolbox.Logstash
{
    public static class LogstashServiceCollectionsExtension
    {
        public static IServiceCollection AddLogstashLogging(this IServiceCollection services, Action<LogstashOptions> setupAction)
        {
            if ( setupAction == null ) throw new ArgumentNullException(nameof(setupAction), $"{nameof(setupAction)} cannot be null.");

            services.Configure(setupAction);
            RegisterServices(services);

            return services;
        }

        public static IServiceCollection AddLogstashLogging(this IServiceCollection services, IConfiguration config)
        {
            if ( config == null ) throw new ArgumentNullException(nameof(config), $"{nameof(config)} cannot be null.");

            services.Configure<LogstashOptions>(config);
            RegisterServices(services);

            return services;
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.TryAddSingleton<LogstashHttpLoggerProvider, LogstashHttpLoggerProvider>();
            services.TryAddTransient<DotNetWebClientProxy, DotNetWebClientProxy>();
        }
    }
}
