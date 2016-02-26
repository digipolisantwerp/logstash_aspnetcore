using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Toolbox.Logstash.Options.Internal;

namespace Toolbox.Logstash.UnitTests
{
    public class TestConfigFactory
    {
        public static KeyValuePair<string, string>[] CreateMemoryConfig(string url, string index, LogLevel? level)
        {
            var list = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(Defaults.ConfigKeys.Url, url),
                new KeyValuePair<string, string>(Defaults.ConfigKeys.Index, index)
            };

            if ( level.HasValue ) list.Add(new KeyValuePair<string, string>(Defaults.ConfigKeys.MinimumLevel, level.Value.ToString()));

            return list.ToArray();
        }
    }
}
