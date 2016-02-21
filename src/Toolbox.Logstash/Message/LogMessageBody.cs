using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Toolbox.Logstash.Internal;

namespace Toolbox.Logstash.Message
{
    public class LogMessageBody
    {
        public LogMessageBody(LogStashLevel level = Defaults.Message.Level)
        {
            Level = level;
        }

        [JsonProperty(Required = Required.DisallowNull)]
        public LogMessageUser User { get; set; }

        public LogStashLevel Level { get; set; }

        [MinLength(1)]
        [MaxLength(1048576)]
        [JsonProperty(Required = Required.Always)]
        public string Content { get; set; }

        [MinLength(1)]
        [MaxLength(32)]
        [JsonProperty(Required = Required.Always)]
        public string VersionNumber { get; set; }
    }
}
