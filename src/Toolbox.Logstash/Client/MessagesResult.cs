using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Toolbox.Logstash.Client
{
    public class MessagesResult
    {
        public int Total { get; set; }

        public List<LogMessage> Messages { get; set; }
    }
}
