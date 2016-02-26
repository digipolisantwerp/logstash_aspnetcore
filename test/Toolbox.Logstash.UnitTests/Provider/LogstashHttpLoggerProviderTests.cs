using System;
using Moq;
using Toolbox.Logstash.Loggers;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Provider
{
    public class LogstashHttpLoggerProviderTests
    {
        [Fact]
        private void OptionsNullRaisesArgumentNullException()
        {
            LogstashOptions nullOptions = null;
            var ex = Assert.Throws<ArgumentNullException>(() => new LogstashHttpLoggerProvider(nullOptions));
            Assert.Equal("options", ex.ParamName);
        }

        [Fact]
        private void OptionsIsSet()
        {
            var options = new LogstashOptions();
            var provider = new LogstashHttpLoggerProvider(options);
            Assert.Same(options, provider.Options);
        }

        [Fact]
        private void LoggerIsSet()
        {
            var options = new LogstashOptions();
            var logger = Mock.Of<ILogstashLogger>();

            var provider = new LogstashHttpLoggerProvider(options, logger);
            Assert.Same(logger, provider.Logger);
        }
    }
}
