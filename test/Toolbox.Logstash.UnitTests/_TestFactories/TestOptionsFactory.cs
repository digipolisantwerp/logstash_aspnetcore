using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Toolbox.Logstash.Options.Internal;

namespace Toolbox.Logstash.UnitTests
{
    public class TestOptionsFactory
    {
        public static KeyValuePair<string, string>[] CreateMemoryConfigValues(string appId, string url, string index, LogLevel? level = null)
        {
            var list = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(Defaults.ConfigKeys.AppId, appId),
                new KeyValuePair<string, string>(Defaults.ConfigKeys.Url, url),
                new KeyValuePair<string, string>(Defaults.ConfigKeys.Index, index)
            };

            if ( level.HasValue ) list.Add(new KeyValuePair<string, string>(Defaults.ConfigKeys.MinimumLevel, level.Value.ToString()));

            return list.ToArray();
        }

        public static IConfiguration CreateMemoryConfig(string appId, string url, string index, LogLevel? level = null)
        {
            var values = CreateMemoryConfigValues(appId, url, index, level);
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(values);
            return configBuilder.Build();
        }

        public static Action<LogstashOptions> CreateSetupAction(string appId, string Url, string index, LogLevel level)
        {
            return options =>
            {
                options.AppId = appId;
                options.Index = index;
                options.MinimumLevel = level;
                options.Url = Url;
            };
        }
    }
}
