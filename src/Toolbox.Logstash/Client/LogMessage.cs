using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Toolbox.Logstash.Client
{
    public class LogMessage
    {
        public LogMessage()
        {

        }

        public Infrastructure Infrastructure
        {
            get;
            set;
        }

        public Message Message
        {
            get;
            set;
        }
    }

    /// <summary>
    /// infrastructuur
    /// - flow initiator
    /// 	- correlation ID(enkel voor apps): request/call binnen redactie app, bv aanmaken notificatie bv GUID 002-000
    /// 	- app ID: redactie app, bv GUID 001-000...
    /// - timestamp
    /// - logging source
    /// - app ID: redactie app, bv GUID 001-000... - ESB past dit aan naar ESB app ID bv GUID 00a-000... 
    /// - module ID(sub id van app of sequence voor ESB)
    /// - IP adres en/of hostname van de host
    /// - proces id
    /// - thread id
    /// - index(config of endpoint)
    /// - log level(trace, info, debug, error, warning)
    /// - versienummer/string van de shipper API ingevuld door de shipper
    /// </summary>
    public class Infrastructure
    {
        public Infrastructure()
        {

        }

        [JsonProperty(Required = Required.Always)]
        public FlowInitiator FlowInitiator
        {
            get;
            set;
        }

        [JsonProperty(Required = Required.Always)]
        [DataType(DataType.DateTime)]
        public DateTime TimeStamp
        {
            get;
            set;
        }

        [JsonProperty(Required = Required.Always)]
        [MaxLength(1024)]
        [MinLength(1)]
        public string LoggingSource
        {
            get;
            set;
        }

        [JsonProperty(Required = Required.Always)]
        [RegularExpression(@"(?i:[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12})")]
        public Guid AppID
        {
            get;
            set;
        }

        [JsonProperty(Required = Required.Always)]
        //[JsonConverter(typeof(IPAddressConverter))]
        [RegularExpression(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b")]
        public string IPAddress
        {
            get;
            set;
        }

        [JsonProperty(Required = Required.Always)]
        //[RegularExpression(@"^[0-9]+$")]
        public string ProcessID
        {
            get;
            set;
        }

        [JsonProperty(Required = Required.Always)]
        //[RegularExpression(@"^[0-9]+$")]
        public string ThreadID
        {
            get;
            set;
        }

        [JsonProperty(Required = Required.DisallowNull)]
        [MaxLength(32)]
        [MinLength(1)]
        public string Index
        {
            get;
            set;
        }

        [JsonProperty(Required = Required.Always)]
        [EnumDataType(typeof(LogLevel))]
        public LogLevel LogLevel
        {
            get;
            set;
        }

        [JsonProperty(Required = Required.Always)]
        [MaxLength(32)]
        [MinLength(1)]
        //[RegularExpression(@"^[0-9]+$")]
        public string VersionNumber
        {
            get;
            set;
        }
    }

    public class FlowInitiator
    {
        public FlowInitiator()
        {

        }

        [JsonProperty(Required = Required.Always)]
        [RegularExpression(@"(?i:[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12})")]
        public Guid CorrelationID
        {
            get;
            set;
        }

        [JsonProperty(Required = Required.Always)]
        [RegularExpression(@"(?i:[0-9a-f]{8}[-]?([0-9a-f]{4}[-]?){3}[0-9a-f]{12})")]
        public Guid ApplicationID
        {
            get;
            set;
        }
    }

    public enum LogLevel
    {
        TRACE,
        INFO,
        DEBUG,
        ERROR,
        WARNING
    }

    /// <summary>
    /// message (business logica)
    /// - user ID ingelogde gebruiker
    ///     - optioneel
    ///     - ip adres
    ///     - user id(bv ICA)
    /// - content
    /// - versienummer/string
    /// </summary>
    public class Message
    {
        public Message()
        {

        }

        [JsonProperty(Required = Required.DisallowNull)]
        public User User
        {
            get;
            set;
        }

        [JsonProperty(Required = Required.Always)]
        [MaxLength(1048576)]
        [MinLength(1)]
        public string Content
        {
            get;
            set;
        }

        [JsonProperty(Required = Required.Always)]
        [MaxLength(32)]
        [MinLength(1)]
        public string VersionNumber
        {
            get;
            set;
        }
    }

    public class User
    {
        public User()
        {

        }

        [JsonProperty(Required = Required.AllowNull)]
        [MaxLength(256)]
        [MinLength(1)]
        public string UserID
        {
            get;
            set;
        }

        [JsonProperty(Required = Required.AllowNull)]
        [RegularExpression(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b")]
        public string IPAddress
        {
            get;
            set;
        }
    }


}
