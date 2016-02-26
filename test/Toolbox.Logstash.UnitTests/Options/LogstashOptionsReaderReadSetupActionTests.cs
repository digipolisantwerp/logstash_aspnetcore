using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Toolbox.Logstash.Options;
using Toolbox.Logstash.Options.Internal;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Options
{
    public class LogstashOptionsReaderReadSetupActionTests
    {
        [Fact]
        private void SetupActionNullRaisesArgumentNullException()
        {
            Action<LogstashOptions> nullAction = null;
            var ex = Assert.Throws<ArgumentNullException>(() => LogstashOptionsReader.Read(nullAction));
            Assert.Equal("setupAction", ex.ParamName);
        }

        [Fact]
        private void UrlNullRaisesInvalidOptionException()
        {
            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(options => 
            {
                options.Url = null;
                options.Index = "index";
                options.MinimumLevel = LogLevel.Information;
            }));
            Assert.Equal(Defaults.ConfigKeys.Url, ex.OptionKey);
            Assert.Equal("(null)", ex.OptionValue);
        }

        [Fact]
        private void UrlEmptyRaisesInvalidOptionException()
        {
            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(options =>
            {
                options.Url = "";
                options.Index = "index";
                options.MinimumLevel = LogLevel.Information;
            }));
            Assert.Equal(Defaults.ConfigKeys.Url, ex.OptionKey);
            Assert.Equal("", ex.OptionValue);
        }

        [Fact]
        private void UrlWhitespaceRaisesInvalidOptionException()
        {
            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(options =>
            {
                options.Url = "  ";
                options.Index = "index";
                options.MinimumLevel = LogLevel.Information;
            }));
            Assert.Equal(Defaults.ConfigKeys.Url, ex.OptionKey);
            Assert.Equal("  ", ex.OptionValue);
        }

        [Fact]
        private void InvalidUriRaisesInvalidOptionException()
        {
            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(options =>
            {
                options.Url = "abcde";
                options.Index = "index";
                options.MinimumLevel = LogLevel.Information;
            }));
            Assert.Equal(Defaults.ConfigKeys.Url, ex.OptionKey);
            Assert.Equal("abcde", ex.OptionValue);
        }

        [Fact]
        private void IndexNullRaisesInvalidOptionException()
        {
            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(options =>
            {
                options.Url = "http://localhost";
                options.Index = null;
                options.MinimumLevel = LogLevel.Information;
            }));
            Assert.Equal(Defaults.ConfigKeys.Index, ex.OptionKey);
            Assert.Equal("(null)", ex.OptionValue);
        }

        [Fact]
        private void IndexEmptyRaisesInvalidOptionException()
        {
            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(options =>
            {
                options.Url = "http://localhost";
                options.Index = "";
                options.MinimumLevel = LogLevel.Information;
            }));
            Assert.Equal(Defaults.ConfigKeys.Index, ex.OptionKey);
            Assert.Equal("", ex.OptionValue);
        }

        [Fact]
        private void IndexWhitespaceRaisesInvalidOptionException()
        {
            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(options =>
            {
                options.Url = "http://localhost";
                options.Index = "  ";
                options.MinimumLevel = LogLevel.Information;
            }));
            Assert.Equal(Defaults.ConfigKeys.Index, ex.OptionKey);
            Assert.Equal("  ", ex.OptionValue);
        }

        [Fact]
        private void MinimumLevelNullSetsToMinimum()
        {
            var logstashOptions = LogstashOptionsReader.Read(options =>
            {
                options.Url = "http://localhost";
                options.Index = "index";
            });

            Assert.Equal(0, (int)logstashOptions.MinimumLevel);               // the levels are changed in RC2, so this test will probably need to be updated
        }
    }
}
