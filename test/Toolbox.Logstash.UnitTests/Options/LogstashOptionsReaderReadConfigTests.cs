using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Toolbox.Logstash.Options;
using Toolbox.Logstash.Options.Internal;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Options
{
    public class LogstashOptionsReaderReadConfigTests
    {
        [Fact]
        private void OptionsNullRaisesArgumentNullException()
        {
            IConfiguration nullConfig = null;
            var ex = Assert.Throws<ArgumentNullException>(() => LogstashOptionsReader.Read(nullConfig));
            Assert.Equal("config", ex.ParamName);
        }

        [Fact]
        private void UrlNullRaisesInvalidOptionException()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(TestConfigFactory.CreateMemoryConfig(null, "index", LogLevel.Error));
            var config = configBuilder.Build();

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Url, ex.OptionKey);
            Assert.Equal("(null)", ex.OptionValue);
        }

        [Fact]
        private void UrlEmptyRaisesInvalidOptionException()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(TestConfigFactory.CreateMemoryConfig("", "index", LogLevel.Error));
            var config = configBuilder.Build();

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Url, ex.OptionKey);
            Assert.Equal("", ex.OptionValue);
        }

        [Fact]
        private void UrlWhitespaceRaisesInvalidOptionException()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(TestConfigFactory.CreateMemoryConfig("  ", "index", LogLevel.Error));
            var config = configBuilder.Build();

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Url, ex.OptionKey);
            Assert.Equal("  ", ex.OptionValue);
        }

        [Fact]
        private void IndexNullRaisesInvalidOptionException()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(TestConfigFactory.CreateMemoryConfig("http://localhost", null, LogLevel.Error));
            var config = configBuilder.Build();

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Index, ex.OptionKey);
            Assert.Equal("(null)", ex.OptionValue);
        }

        [Fact]
        private void IndexEmptyRaisesInvalidOptionException()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(TestConfigFactory.CreateMemoryConfig("http://localhost", "", LogLevel.Error));
            var config = configBuilder.Build();

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Index, ex.OptionKey);
            Assert.Equal("", ex.OptionValue);
        }

        [Fact]
        private void IndexWhitespaceRaisesInvalidOptionException()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(TestConfigFactory.CreateMemoryConfig("http://localhost", "  ", LogLevel.Error));
            var config = configBuilder.Build();

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Index, ex.OptionKey);
            Assert.Equal("  ", ex.OptionValue);
        }

        [Fact]
        private void MinimumLevelNullSetsToMinimum()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(TestConfigFactory.CreateMemoryConfig("http://localhost", "index", null));
            var config = configBuilder.Build();
            
            var options = LogstashOptionsReader.Read(config);
            Assert.Equal(0, (int)options.MinimumLevel);               // the levels are changed in RC2, so this test will probably need to be updated
        }
    }
}
