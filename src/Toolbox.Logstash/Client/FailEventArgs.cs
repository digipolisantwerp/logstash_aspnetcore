using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Toolbox.Logstash.Client
{
    public class FailEventArgs : EventArgs
    {
        public FailEventArgs(LogMessage message, Exception error)
        {
            Message = message;
            Error = error;
        }

        public LogMessage Message { get; set; }

        public Exception Error { get; set; }
    }
}
