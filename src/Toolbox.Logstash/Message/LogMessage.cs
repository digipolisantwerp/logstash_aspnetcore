using System;
using Newtonsoft.Json;
using Toolbox.Logstash.Options.Internal;

namespace Toolbox.Logstash.Message
{
    public class LogMessage
    {
        public LogMessage(LogStashLevel level = Defaults.Message.Level)
        {
            Header = new LogMessageHeader();
            Body = new LogMessageBody(level);
        }

        [JsonProperty(Required = Required.Always)]
        public LogMessageHeader Header { get; set; }

        [JsonProperty(Required = Required.Always)]
        public LogMessageBody Body { get; set; }
    }
}
