using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Toolbox.Logstash.Message
{
    public class LogMessageUser
    {
        [MinLength(1)]
        [MaxLength(256)]
        [JsonProperty(Required = Required.AllowNull)]
        public string UserId { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        [RegularExpression(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b")]
        public string IPAddress { get; set; }
    }
}
