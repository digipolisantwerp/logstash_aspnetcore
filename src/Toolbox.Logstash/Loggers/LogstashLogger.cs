using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Extensions.Logging;
using Toolbox.Logstash.Message;
using Toolbox.Logstash.Options.Internal;

namespace Toolbox.Logstash.Loggers
{
    public class LogstashLogger : ILogger
    {
        public LogstashLogger(LogstashOptions options, ILogstashLogger logger)
        {
            if ( options == null ) throw new ArgumentNullException(nameof(options), $"{nameof(options)} cannot be null.");
            if ( logger == null ) throw new ArgumentNullException(nameof(logger), $"{nameof(logger)} cannot be null.");
            Options = options;
            Logger = logger;
            _localIPAddress = GetLocalIPAddress();
            _currentProcessId = GetCurrentProcessId();
        }

        public LogstashOptions Options { get; private set; }
        public ILogstashLogger Logger {get; private set; }
        
        private string _localIPAddress;
        private string _currentProcessId;

        public IDisposable BeginScopeImpl(object state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            // The logstash client doesn't implement the concept of enabled/disabled log levels.
            return true;
        }

        public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            if ( state == null ) return;
            if ( string.IsNullOrWhiteSpace(state.ToString()) ) return;

            string message;
            if ( formatter != null )
                message = formatter(state, exception);
            else
                message = Microsoft.Extensions.Logging.LogFormatter.Formatter(state, exception);

            if ( !string.IsNullOrEmpty(message) )
            {
                // ToDo (SVB) : all of this to LogMessageBuilder class ?

                var logstashLevel = LogLevelToLogStashLevel(logLevel);
                var logMessage = new LogMessage(logstashLevel);

                logMessage.Header.Correlation = new Correlation() { ApplicationId = "appid-todo", CorrelationId = "correlationid-todo" };
                logMessage.Header.Index = Options.Index;
                logMessage.Header.Source = new LogMessageSource(Options.AppId, "componentid-todo");
                logMessage.Header.TimeStamp = DateTime.Now;
                logMessage.Header.VersionNumber = Defaults.Message.HeaderVersion;
                logMessage.Header.IPAddress = _localIPAddress;
                logMessage.Header.ProcessId = _currentProcessId;
                logMessage.Header.ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();

                logMessage.Body.User = new LogMessageUser() { UserId = Thread.CurrentPrincipal.Identity.Name, IPAddress = null };       // ToDo (SVB) : where does user's ip address come from?
                //logMessage.Body.VersionNumber = Options.MessageVersion;       // ??
                //logMessage.Body.Content = Serialize(state);     // ??

                Logger.Log(logMessage);
            }
        }

        private LogStashLevel LogLevelToLogStashLevel(LogLevel logLevel)
        {
            var level = LogStashLevel.Information;

            switch ( logLevel )
            {
                case LogLevel.Debug:
                    level = LogStashLevel.Debug;
                    break;
                case LogLevel.Verbose:
                    level = LogStashLevel.Trace;
                    break;
                case LogLevel.Information:
                    level = LogStashLevel.Information;
                    break;
                case LogLevel.Warning:
                    level = LogStashLevel.Warning;
                    break;
                case LogLevel.Error:
                    level = LogStashLevel.Error;
                    break;
                case LogLevel.Critical:
                    level = LogStashLevel.Critical;
                    break;
                case LogLevel.None:
                    level = LogStashLevel.None;
                    break;
            }

            return level;
        }

        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach ( var ip in host.AddressList )
            {
                if ( ip.AddressFamily == AddressFamily.InterNetwork )
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        public string GetCurrentProcessId()
        {
            return Process.GetCurrentProcess().Id.ToString();
        }
    }
}
