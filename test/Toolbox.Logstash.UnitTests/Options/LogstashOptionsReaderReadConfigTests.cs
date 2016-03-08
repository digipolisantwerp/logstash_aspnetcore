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
            var config = TestOptionsFactory.CreateMemoryConfig("myApp", null, "index", LogLevel.Error);

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Url, ex.OptionKey);
            Assert.Equal("(null)", ex.OptionValue);
        }

        [Fact]
        private void UrlEmptyRaisesInvalidOptionException()
        {
            var config = TestOptionsFactory.CreateMemoryConfig("myApp", "", "index", LogLevel.Error);

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Url, ex.OptionKey);
            Assert.Equal("", ex.OptionValue);
        }

        [Fact]
        private void UrlWhitespaceRaisesInvalidOptionException()
        {
            var config = TestOptionsFactory.CreateMemoryConfig("myApp", "  ", "index", LogLevel.Error);
            
            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Url, ex.OptionKey);
            Assert.Equal("  ", ex.OptionValue);
        }

        [Fact]
        private void InvalidUriRaisesInvalidOptionException()
        {
            var config = TestOptionsFactory.CreateMemoryConfig("myApp", "abcdefg", "index", LogLevel.Error);

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Url, ex.OptionKey);
            Assert.Equal("abcdefg", ex.OptionValue);
        }

        [Fact]
        private void IndexNullRaisesInvalidOptionException()
        {
            var config = TestOptionsFactory.CreateMemoryConfig("myApp", "http://localhost", null, LogLevel.Error);

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Index, ex.OptionKey);
            Assert.Equal("(null)", ex.OptionValue);
        }

        [Fact]
        private void IndexEmptyRaisesInvalidOptionException()
        {
            var config = TestOptionsFactory.CreateMemoryConfig("myApp", "http://localhost", "", LogLevel.Error);

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Index, ex.OptionKey);
            Assert.Equal("", ex.OptionValue);
        }

        [Fact]
        private void IndexWhitespaceRaisesInvalidOptionException()
        {
            var config = TestOptionsFactory.CreateMemoryConfig("myApp", "http://localhost", "  ", LogLevel.Error);

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.Index, ex.OptionKey);
            Assert.Equal("  ", ex.OptionValue);
        }

        [Fact]
        private void MinimumLevelNullSetsToMinimum()
        {
            var config = TestOptionsFactory.CreateMemoryConfig("myApp", "http://localhost", "index", null);
            
            var options = LogstashOptionsReader.Read(config);
            Assert.Equal(0, (int)options.MinimumLevel);               // the levels are changed in RC2, so this test will probably need to be updated
        }

        [Fact]
        private void AppIdNullRaisesInvalidOptionException()
        {
            var config = TestOptionsFactory.CreateMemoryConfig(null, "http://localhost", "index", LogLevel.Error);

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.AppId, ex.OptionKey);
            Assert.Equal("(null)", ex.OptionValue);
        }

        [Fact]
        private void AppIdEmptyRaisesInvalidOptionException()
        {
            var config = TestOptionsFactory.CreateMemoryConfig("", "http://localhost", "index", LogLevel.Error);

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.AppId, ex.OptionKey);
            Assert.Equal("", ex.OptionValue);
        }
         
        [Fact]
        private void AppIdWhitespaceRaisesInvalidOptionException()
        {
            var config = TestOptionsFactory.CreateMemoryConfig("  ", "http://localhost", "index", LogLevel.Error);

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(config));
            Assert.Equal(Defaults.ConfigKeys.AppId, ex.OptionKey);
            Assert.Equal("  ", ex.OptionValue);
        }
    }
}
