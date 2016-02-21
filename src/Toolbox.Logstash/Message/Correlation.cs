using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Toolbox.Logstash.Message
{
    public class Correlation
    {
        [MinLength(1)]
        [MaxLength(1024)]
        [JsonProperty(Required = Required.Always)]
        public string CorrelationId { get; set; }

        [MinLength(1)]
        [MaxLength(1024)]
        [JsonProperty(Required = Required.Always)]
        public string ApplicationId { get; set; }
    }
}
