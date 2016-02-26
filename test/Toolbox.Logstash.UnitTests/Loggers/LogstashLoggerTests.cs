using System;
using Moq;
using Toolbox.Logstash.Loggers;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Loggers
{
    public class LogstashLoggerTests
    {
        [Fact]
        private void OptionsNullRaisesArgumentNullException()
        {
            var innerLogger = Mock.Of<ILogstashLogger>();
            var ex = Assert.Throws<ArgumentNullException>(() => new LogstashLogger(null, innerLogger));
            Assert.Equal("options", ex.ParamName);
        }

        [Fact]
        private void LoggerNullRaisesArgumentNullException()
        {
            var options = new LogstashOptions();
            var ex = Assert.Throws<ArgumentNullException>(() => new LogstashLogger(options, null));
            Assert.Equal("logger", ex.ParamName);
        }

        [Fact]
        private void OptionsIsSet()
        {
            var options = new LogstashOptions();
            var innerLogger = Mock.Of<ILogstashLogger>();

            var logger = new LogstashLogger(options, innerLogger);

            Assert.Same( options, logger.Options);
        }

        [Fact]
        private void LoggerIsSet()
        {
            var options = new LogstashOptions();
            var innerLogger = Mock.Of<ILogstashLogger>();

            var logger = new LogstashLogger(options, innerLogger);

            Assert.Same(innerLogger, logger.Logger);
        }
    }
}
