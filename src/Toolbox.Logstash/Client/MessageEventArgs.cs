using System;
using Toolbox.Logstash.Message;

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
