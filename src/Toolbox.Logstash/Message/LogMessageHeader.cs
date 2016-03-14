using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Toolbox.Logstash.Message
{
    public class LogMessageHeader
    {
        [JsonProperty(Required = Required.Always)]
        public LogMessageCorrelation Correlation { get; set; }

        [DataType(DataType.DateTime)]
        [JsonProperty(Required = Required.Always)]
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        [JsonProperty(Required = Required.Always)]
        public LogMessageSource Source { get; set; }

        [RegularExpression(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b")]
        public string IPAddress { get; set; }

        [RegularExpression(@"^\d$")] 
        public string ProcessId { get; set; }

        [RegularExpression(@"^\d$")]
        public string ThreadId { get; set; }
        
        [MinLength(1)]
        [MaxLength(32)]
        [JsonProperty(Required = Required.DisallowNull)]
        public string Index { get; set; }
        
        [MinLength(1)]
        [MaxLength(32)]
        [JsonProperty(Required = Required.Always)]
        public string VersionNumber { get; set; }
    }
}
