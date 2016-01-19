using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Toolbox.Logstash
{
    public static class LogstashHTTPInputLoggerFactoryExtensions
    {
        public static ILoggerFactory AddLogstashHTTPInput(this ILoggerFactory factory, Guid logId)
        {
            factory.AddProvider(new LogstashHTTPInputLoggerProvider(logId));
            return factory;
        }
    }
}
