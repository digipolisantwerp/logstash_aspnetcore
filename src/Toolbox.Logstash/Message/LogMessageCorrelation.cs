using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Toolbox.Logstash.Message
{
    public class LogMessageCorrelation
    {
        public LogMessageCorrelation(string applicationId = null, string correlationId = null)
        {
            ApplicationId = applicationId;
            CorrelationId = correlationId;
        }

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
