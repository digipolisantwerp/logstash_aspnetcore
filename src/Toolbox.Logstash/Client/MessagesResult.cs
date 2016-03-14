using System;
using System.Collections.Generic;
using Toolbox.Logstash.Message;

namespace Toolbox.Logstash.Client
{
    public class MessagesResult
    {
        public int Total { get; set; }

        public List<LogMessage> Messages { get; set; }
    }
}
