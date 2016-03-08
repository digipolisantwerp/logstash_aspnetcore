using System;
using Moq;
using Toolbox.Logstash.Loggers;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Loggers
{
    public class LogstashLoggerTests
    {
        [Fact]
        private void LoggerNullRaisesArgumentNullException()
        {
            var options = new LogstashOptions();
            var ex = Assert.Throws<ArgumentNullException>(() => new LogstashLogger(options));
            Assert.Equal("logger", ex.ParamName);
        }

        [Fact]
        private void LoggerIsSet()
        {
            var options = new LogstashOptions();
            var innerLogger = Mock.Of<ILogstashHttpLogger>();

            var logger = new LogstashLogger(options, innerLogger);

            Assert.Same(innerLogger, logger.Logger);
        }
    }
}
