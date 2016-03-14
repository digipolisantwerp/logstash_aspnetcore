using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Toolbox.Logstash.Message
{
    public class LogMessageSource
    {
        public LogMessageSource(string applicationId = null, string componentId = null)
        {
            ApplicationId = applicationId;
            ComponentId = componentId;
        }

        [MinLength(1)]
        [MaxLength(1024)]
        [JsonProperty(Required = Required.Always)]
        public string ApplicationId { get; set; }

        [MinLength(1)]
        [MaxLength(1024)]
        public string ComponentId { get; set; }
    }
}
