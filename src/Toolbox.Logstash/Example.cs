using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Toolbox.Logstash.Client;

namespace Toolbox.Logstash
{
    public class Example
    {
        public static void Main(string[] args)
        {
            var factory = new LoggerFactory();
            factory.MinimumLevel = Microsoft.Extensions.Logging.LogLevel.Debug;
            var logger = factory.CreateLogger("MyLog");
            factory.AddLogstashHTTPInput(new Guid("bbe87cfc-526c-4240-a59f-ef1b61e2bed6"));

            for (int i = 0; i < 500; i++)
            {
                Task.Run(() => logger.Log(Microsoft.Extensions.Logging.LogLevel.Debug, 100, BuildLogMessage(), null, null));
            }
            System.Console.ReadLine();
        }

        public static LogMessage BuildLogMessage()
        {
            LogMessage log = new LogMessage();
            Infrastructure infra = new Infrastructure();
            Message message = new Message();

            log.Infrastructure = infra;
            log.Message = message;

            infra.AppID = Guid.NewGuid();
            infra.FlowInitiator = new FlowInitiator();
            infra.FlowInitiator.ApplicationID = Guid.NewGuid();
            infra.FlowInitiator.CorrelationID = Guid.NewGuid();
            infra.Index = "new_index";
            infra.IPAddress = IPAddress.Loopback.ToString();
            infra.LoggingSource = "logging_source";
            infra.LogLevel = Client.LogLevel.ERROR;
            infra.ProcessID = "135";
            infra.ThreadID = "15";
            infra.TimeStamp = DateTime.Now;
            infra.VersionNumber = "v1.0.2";

            message.Content = "business_content";
            message.User = new User();
            message.User.IPAddress = IPAddress.Loopback.ToString();
            message.User.UserID = @"ICA\EX01913";
            message.VersionNumber = "v1.0.3";

            return log;

        }
    }
}
