using Microsoft.Extensions.Logging;
using System;

namespace Toolbox.Logstash
{
    public static class LogstashHttpLoggerFactoryExtensions
    {
        public static ILoggerFactory AddLogstashHttp(this ILoggerFactory factory, Guid logId)
        {
            factory.AddProvider(new LogstashHttpLoggerProvider(logId));
            return factory;
        }
    }
}
