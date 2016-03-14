using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Toolbox.Logstash.Options.Internal;

namespace Toolbox.Logstash.Options
{
    public class LogstashOptionsReader
    {
        public static LogstashOptions Read(IConfiguration config)
        {
            if ( config == null ) throw new ArgumentNullException(nameof(config), $"{nameof(config)} cannot be null.");

            var options = new LogstashOptions()
            {
                AppId = config.Get<string>(Defaults.ConfigKeys.AppId),
                Url = config.Get<string>(Defaults.ConfigKeys.Url),
                Index = config.Get<string>(Defaults.ConfigKeys.Index),
                MinimumLevel = config.Get<LogLevel>(Defaults.ConfigKeys.MinimumLevel)
            };

            Validate(options);

            return options;
        }

        public static LogstashOptions Read(Action<LogstashOptions> setupAction)
        {
            if ( setupAction == null ) throw new ArgumentNullException(nameof(setupAction), $"{nameof(setupAction)} cannot be null.");

            var options = new LogstashOptions();
            setupAction.Invoke(options);

            Validate(options);

            return options;
        }

        private static void Validate(LogstashOptions options)
        {
            if ( String.IsNullOrWhiteSpace(options.AppId) ) throw new InvalidOptionException(Defaults.ConfigKeys.AppId, options.AppId, "Logging AppId is mandatory.");
            if ( String.IsNullOrWhiteSpace(options.Url) ) throw new InvalidOptionException(Defaults.ConfigKeys.Url, options.Url, "Logging Url is mandatory.");
            if ( String.IsNullOrWhiteSpace(options.Index) ) throw new InvalidOptionException(Defaults.ConfigKeys.Index, options.Index, "Logging Index is mandatory.");

            try
            {
                var uri = new Uri(options.Url);
            }
            catch (UriFormatException)
            {
                throw new InvalidOptionException(Defaults.ConfigKeys.Url, options.Url, "Logging Url is not a valid uri.");
            }
        }
    }
}
