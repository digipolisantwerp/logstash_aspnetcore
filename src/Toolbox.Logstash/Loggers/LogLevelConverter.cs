using System;
using Microsoft.Extensions.Logging;

namespace Toolbox.Logstash.Loggers
{
    public static class LogLevelConverter
    {
        public static LogStashLevel ToLogStashLevel(LogLevel logLevel)
        {
            var level = LogStashLevel.Information;

            switch ( logLevel )
            {
                case LogLevel.Debug:
                    level = LogStashLevel.Debug;
                    break;
                case LogLevel.Verbose:
                    level = LogStashLevel.Trace;
                    break;
                case LogLevel.Information:
                    level = LogStashLevel.Information;
                    break;
                case LogLevel.Warning:
                    level = LogStashLevel.Warning;
                    break;
                case LogLevel.Error:
                    level = LogStashLevel.Error;
                    break;
                case LogLevel.Critical:
                    level = LogStashLevel.Critical;
                    break;
                case LogLevel.None:
                    level = LogStashLevel.None;
                    break;
            }

            return level;
        }
    }
}
