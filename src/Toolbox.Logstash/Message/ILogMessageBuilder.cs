using System;
using Microsoft.Extensions.Logging;

namespace Toolbox.Logstash.Message
{
    public interface ILogMessageBuilder
    {
        LogMessage Build(string loggerName, LogLevel level, object state, Exception exception, Func<object, Exception, string> formatter = null);
    }
}
