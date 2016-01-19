using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Toolbox.Logstash.Client
{
    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(LogMessage message)
        {
            Message = message;
        }

        public LogMessage Message { get; set; }
    }
}
