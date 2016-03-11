using Microsoft.Extensions.Logging;

namespace Toolbox.Logstash.Loggers
{
    public interface ILogLevelConverter
    {
        LogstashLevel ToLogStashLevel(LogLevel logLevel);
    }
}