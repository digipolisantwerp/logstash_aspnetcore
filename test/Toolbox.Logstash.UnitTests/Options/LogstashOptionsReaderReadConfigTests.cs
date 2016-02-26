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
            configBuilder.AddInMemoryCollection(TestOptionsFactory.CreateMemoryConfig("myApp", null, "index", LogLevel.Error));
            var config = configBuilder.Build();

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Url, ex.OptionKey);
            Assert.Equal("(null)", ex.OptionValue);
        }

        [Fact]
        private void UrlEmptyRaisesInvalidOptionException()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(TestOptionsFactory.CreateMemoryConfig("myApp", "", "index", LogLevel.Error));
            var config = configBuilder.Build();

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Url, ex.OptionKey);
            Assert.Equal("", ex.OptionValue);
        }

        [Fact]
        private void UrlWhitespaceRaisesInvalidOptionException()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(TestOptionsFactory.CreateMemoryConfig("myApp", "  ", "index", LogLevel.Error));
            var config = configBuilder.Build();

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Url, ex.OptionKey);
            Assert.Equal("  ", ex.OptionValue);
        }

        [Fact]
        private void InvalidUriRaisesInvalidOptionException()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(TestOptionsFactory.CreateMemoryConfig("myApp", "abcdefg", "index", LogLevel.Error));
            var config = configBuilder.Build();

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Url, ex.OptionKey);
            Assert.Equal("abcdefg", ex.OptionValue);
        }

        [Fact]
        private void IndexNullRaisesInvalidOptionException()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(TestOptionsFactory.CreateMemoryConfig("myApp", "http://localhost", null, LogLevel.Error));
            var config = configBuilder.Build();

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Index, ex.OptionKey);
            Assert.Equal("(null)", ex.OptionValue);
        }

        [Fact]
        private void IndexEmptyRaisesInvalidOptionException()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(TestOptionsFactory.CreateMemoryConfig("myApp", "http://localhost", "", LogLevel.Error));
            var config = configBuilder.Build();

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Index, ex.OptionKey);
            Assert.Equal("", ex.OptionValue);
        }

        [Fact]
        private void IndexWhitespaceRaisesInvalidOptionException()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(TestOptionsFactory.CreateMemoryConfig("myApp", "http://localhost", "  ", LogLevel.Error));
            var config = configBuilder.Build();

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Index, ex.OptionKey);
            Assert.Equal("  ", ex.OptionValue);
        }

        [Fact]
        private void MinimumLevelNullSetsToMinimum()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(TestOptionsFactory.CreateMemoryConfig("myApp", "http://localhost", "index", null));
            var config = configBuilder.Build();
            
            var options = LogstashOptionsReader.Read(config);
            Assert.Equal(0, (int)options.MinimumLevel);               // the levels are changed in RC2, so this test will probably need to be updated
        }

        [Fact]
        private void AppIdNullRaisesInvalidOptionException()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(TestOptionsFactory.CreateMemoryConfig(null, "http://localhost", "index", LogLevel.Error));
            var config = configBuilder.Build();

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.AppId, ex.OptionKey);
            Assert.Equal("(null)", ex.OptionValue);
        }

        [Fact]
        private void AppIdEmptyRaisesInvalidOptionException()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(TestOptionsFactory.CreateMemoryConfig("", "http://localhost", "index", LogLevel.Error));
            var config = configBuilder.Build();

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.AppId, ex.OptionKey);
            Assert.Equal("", ex.OptionValue);
        }
         
        [Fact]
        private void AppIdWhitespaceRaisesInvalidOptionException()
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(TestOptionsFactory.CreateMemoryConfig("  ", "http://localhost", "index", LogLevel.Error));
            var config = configBuilder.Build();

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.AppId, ex.OptionKey);
            Assert.Equal("  ", ex.OptionValue);
        }
    }
}
