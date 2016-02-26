using System;
using Microsoft.Extensions.Logging;

namespace Toolbox.Logstash
{
    public class LogstashOptions
    {
        /// <summary>
        /// The Logstash Index where the logging messages are written to.
        /// </summary>
        public string Index { get; set; }

        /// <summary>
        /// The url of the Logstash HTTP endpoint.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The MinimumLevel of the Logstash logger.
        /// </summary>
        public LogLevel MinimumLevel { get; set; }
    }
}
