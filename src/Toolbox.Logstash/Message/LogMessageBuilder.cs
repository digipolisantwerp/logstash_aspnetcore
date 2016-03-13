using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Extensions.Logging;
using Toolbox.Correlation;
using Toolbox.Logstash.Loggers;
using Toolbox.Logstash.Options.Internal;

namespace Toolbox.Logstash.Message
{
    public class LogMessageBuilder : ILogMessageBuilder
    {
        public LogMessageBuilder(IServiceProvider serviceProvider, ILogLevelConverter logLevelConverter, LogstashOptions options)
        {
            if ( serviceProvider == null ) throw new ArgumentNullException(nameof(serviceProvider), $"{nameof(serviceProvider)} cannot be null.");
            if ( logLevelConverter == null ) throw new ArgumentNullException(nameof(logLevelConverter), $"{nameof(logLevelConverter)} cannot be null.");
            if ( options == null ) throw new ArgumentNullException(nameof(options), $"{nameof(options)} cannot be null.");
            ServiceProvider = serviceProvider;
            LogLevelConverter = logLevelConverter;
            Options = options;
            LocalIPAddress = GetLocalIPAddress();
            CurrentProcess = GetCurrentProcessId();
        }

        internal IServiceProvider ServiceProvider { get; private set; }
        internal ILogLevelConverter LogLevelConverter { get; private set; }
        internal LogstashOptions Options { get; private set; }

        internal string LocalIPAddress { get; private set; }
        internal string CurrentProcess { get; private set; }

        public LogMessage Build(string loggerName, LogLevel logLevel, object state, Exception exception, Func<object, Exception, string> formatter = null)
        {
            if ( state == null && exception == null ) return null;

            
            

            var logstashLevel = LogLevelConverter.ToLogStashLevel(logLevel);
            var logMessage = new LogMessage(logstashLevel); 
            
            string message;
            if ( formatter != null )
                message = formatter(state, exception);
            else
                message = Microsoft.Extensions.Logging.LogFormatter.Formatter(state, exception);

            if ( !string.IsNullOrEmpty(message) )
            {
                logMessage.Header.Correlation = BuildCorrelation();
                logMessage.Header.Index = Options.Index;
                logMessage.Header.Source = new LogMessageSource(Options.AppId, loggerName);
                logMessage.Header.TimeStamp = DateTime.Now;
                logMessage.Header.VersionNumber = Defaults.Message.HeaderVersion;
                logMessage.Header.IPAddress = LocalIPAddress;
                logMessage.Header.ProcessId = CurrentProcess;
                logMessage.Header.ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();

                //logMessage.Body.User = new LogMessageUser() { UserId = Thread.CurrentPrincipal?.Identity?.Name, IPAddress = LocalIPAddress };       // ToDo (SVB) : where does user's ip address come from?
                logMessage.Body.User = new LogMessageUser() { UserId = "ss", IPAddress = LocalIPAddress };
                logMessage.Body.VersionNumber = Options.MessageVersion;
                logMessage.Body.Content = message;
                //logMessage.Body.Content = Serialize(state);     // ??

            }

            return logMessage;
        }

        private LogMessageCorrelation BuildCorrelation()
        {
            var correlationContext = ServiceProvider.GetService(typeof(ICorrelationContext)) as ICorrelationContext;
            if ( correlationContext != null )
                return new LogMessageCorrelation(correlationContext.CorrelationSource, correlationContext.CorrelationId);
            else
                return new LogMessageCorrelation(Options.AppId, Guid.NewGuid().ToString());
        }

        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach ( var ip in host.AddressList )
            {
                if ( ip.AddressFamily == AddressFamily.InterNetwork )
                {
                    return ip.ToString();
                }
            }
            return "unable to determine local IP Address.";
        }

        private string GetCurrentProcessId()
        {
            return Process.GetCurrentProcess().Id.ToString();
        }
    }
}
