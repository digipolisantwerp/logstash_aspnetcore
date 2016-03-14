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
            var setupAction = TestOptionsFactory.CreateSetupAction("myApp", null, "index", LogLevel.Information);

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(setupAction));

            Assert.Equal(Defaults.ConfigKeys.Url, ex.OptionKey);
            Assert.Equal("(null)", ex.OptionValue);
        }

        [Fact]
        private void UrlEmptyRaisesInvalidOptionException()
        {
            var setupAction = TestOptionsFactory.CreateSetupAction("myApp", "", "index", LogLevel.Information);

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(setupAction));

            Assert.Equal(Defaults.ConfigKeys.Url, ex.OptionKey);
            Assert.Equal("", ex.OptionValue);
        }

        [Fact]
        private void UrlWhitespaceRaisesInvalidOptionException()
        {
            var setupAction = TestOptionsFactory.CreateSetupAction("myApp", "  ", "index", LogLevel.Information);

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(setupAction));

            Assert.Equal(Defaults.ConfigKeys.Url, ex.OptionKey);
            Assert.Equal("  ", ex.OptionValue);
        }

        [Fact]
        private void InvalidUriRaisesInvalidOptionException()
        {
            var setupAction = TestOptionsFactory.CreateSetupAction("myApp", "abcde", "index", LogLevel.Information);

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(setupAction));

            Assert.Equal(Defaults.ConfigKeys.Url, ex.OptionKey);
            Assert.Equal("abcde", ex.OptionValue);
        }

        [Fact]
        private void IndexNullRaisesInvalidOptionException()
        {
            var setupAction = TestOptionsFactory.CreateSetupAction("myApp", "http://localhost", null, LogLevel.Information);

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(setupAction));

            Assert.Equal(Defaults.ConfigKeys.Index, ex.OptionKey);
            Assert.Equal("(null)", ex.OptionValue);
        }

        [Fact]
        private void IndexEmptyRaisesInvalidOptionException()
        {
            var setupAction = TestOptionsFactory.CreateSetupAction("myApp", "http://localhost", "", LogLevel.Information);

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(setupAction));

            Assert.Equal(Defaults.ConfigKeys.Index, ex.OptionKey);
            Assert.Equal("", ex.OptionValue);
        }

        [Fact]
        private void IndexWhitespaceRaisesInvalidOptionException()
        {
            var setupAction = TestOptionsFactory.CreateSetupAction("myApp", "http://localhost", "  ", LogLevel.Information);

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(setupAction));

            Assert.Equal(Defaults.ConfigKeys.Index, ex.OptionKey);
            Assert.Equal("  ", ex.OptionValue);
        }

        [Fact]
        private void MinimumLevelNullSetsToMinimum()
        {
            var logstashOptions = LogstashOptionsReader.Read(options =>
            {
                options.AppId = "myApp";
                options.Url = "http://localhost";
                options.Index = "index";
            });

            Assert.Equal(0, (int)logstashOptions.MinimumLevel);               // the levels are changed in RC2, so this test will probably need to be updated
        }

        [Fact]
        private void AppIdNullRaisesInvalidOptionException()
        {
            var setupAction = TestOptionsFactory.CreateSetupAction(null, "http://localhost", "index", LogLevel.Information);

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(setupAction));

            Assert.Equal(Defaults.ConfigKeys.AppId, ex.OptionKey);
            Assert.Equal("(null)", ex.OptionValue);
        }

        [Fact]
        private void AppIdEmptyRaisesInvalidOptionException()
        {
            var setupAction = TestOptionsFactory.CreateSetupAction("", "http://localhost", "index", LogLevel.Information);

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(setupAction));

            Assert.Equal(Defaults.ConfigKeys.AppId, ex.OptionKey);
            Assert.Equal("", ex.OptionValue);
        }

        [Fact]
        private void AppIdWhitespaceRaisesInvalidOptionException()
        {
            var setupAction = TestOptionsFactory.CreateSetupAction("  ", "http://localhost", "index", LogLevel.Information);

            var ex = Assert.Throws<InvalidOptionException>(() => LogstashOptionsReader.Read(setupAction));

            Assert.Equal(Defaults.ConfigKeys.AppId, ex.OptionKey);
            Assert.Equal("  ", ex.OptionValue);
        }
    }
}
