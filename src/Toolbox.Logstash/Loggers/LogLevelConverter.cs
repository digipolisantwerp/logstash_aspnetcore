using System;
using Microsoft.Extensions.Logging;

namespace Toolbox.Logstash.Loggers
{
    public class LogLevelConverter : ILogLevelConverter
    {
        public LogstashLevel ToLogStashLevel(LogLevel logLevel)
        {
            var level = LogstashLevel.Information;

            switch ( logLevel )
            {
                case LogLevel.Debug:
                    level = LogstashLevel.Debug;
                    break;
                case LogLevel.Verbose:
                    level = LogstashLevel.Trace;
                    break;
                case LogLevel.Information:
                    level = LogstashLevel.Information;
                    break;
                case LogLevel.Warning:
                    level = LogstashLevel.Warning;
                    break;
                case LogLevel.Error:
                    level = LogstashLevel.Error;
                    break;
                case LogLevel.Critical:
                    level = LogstashLevel.Critical;
                    break;
                case LogLevel.None:
                    level = LogstashLevel.None;
                    break;
            }

            return level;
        }
    }
}
