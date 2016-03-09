using System;
using Moq;
using Toolbox.Logstash.Loggers;
using Toolbox.Logstash.Message;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Loggers
{
    public class LogstashLoggerTests
    {

        [Fact]
        private void BuilderNullRaisesArgumentNullException()
        {
            var logger = Mock.Of<ILogstashHttpLogger>();
            var ex = Assert.Throws<ArgumentNullException>(() => new LogstashLogger(null, logger));
            Assert.Equal("logMessageBuilder", ex.ParamName);
        }

        [Fact]
        private void LoggerNullRaisesArgumentNullException()
        {
            var builder = Mock.Of<ILogMessageBuilder>();
            var ex = Assert.Throws<ArgumentNullException>(() => new LogstashLogger(builder, null));
            Assert.Equal("logger", ex.ParamName);
        }

        [Fact]
        private void LogMessageBuilderIsSet()
        {
            var builder = Mock.Of<ILogMessageBuilder>();
            var innerLogger = Mock.Of<ILogstashHttpLogger>();

            var logger = new LogstashLogger(builder, innerLogger);

            Assert.Same(builder, logger.LogMessageBuilder);
        }

        [Fact]
        private void LoggerIsSet()
        {
            var builder = Mock.Of<ILogMessageBuilder>();
            var innerLogger = Mock.Of<ILogstashHttpLogger>();

            var logger = new LogstashLogger(builder, innerLogger);

            Assert.Same(innerLogger, logger.Logger);
        }
    }
}
