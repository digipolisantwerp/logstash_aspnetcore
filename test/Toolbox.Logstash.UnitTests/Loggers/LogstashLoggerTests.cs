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
            var ex = Assert.Throws<ArgumentNullException>(() => new LogstashLogger("myLogger", null, logger));
            Assert.Equal("logMessageBuilder", ex.ParamName);
        }

        [Fact]
        private void LoggerNullRaisesArgumentNullException()
        {
            var builder = Mock.Of<ILogMessageBuilder>();
            var ex = Assert.Throws<ArgumentNullException>(() => new LogstashLogger("myLogger", builder, null));
            Assert.Equal("logger", ex.ParamName);
        }

        [Fact]
        private void LogMessageBuilderIsSet()
        {
            var builder = Mock.Of<ILogMessageBuilder>();
            var innerLogger = Mock.Of<ILogstashHttpLogger>();

            var logger = new LogstashLogger("myLogger", builder, innerLogger);

            Assert.Same(builder, logger.LogMessageBuilder);
        }

        [Fact]
        private void LoggerIsSet()
        {
            var builder = Mock.Of<ILogMessageBuilder>();
            var innerLogger = Mock.Of<ILogstashHttpLogger>();

            var logger = new LogstashLogger("myLogger", builder, innerLogger);

            Assert.Same(innerLogger, logger.Logger);
        }


        [Fact]
        private void NameIsSet()
        {
            var builder = Mock.Of<ILogMessageBuilder>();
            var innerLogger = Mock.Of<ILogstashHttpLogger>();

            var logger = new LogstashLogger("myLogger", builder, innerLogger);

            Assert.Equal(logger.Name, "myLogger");
        }

        [Fact]
        private void NameNullInitializesName()
        {
            var builder = Mock.Of<ILogMessageBuilder>();
            var innerLogger = Mock.Of<ILogstashHttpLogger>();

            var logger = new LogstashLogger(null, builder, innerLogger);

            Assert.NotNull(logger.Name);
        }

        [Fact]
        private void NameEmptyInitializesName()
        {
            var builder = Mock.Of<ILogMessageBuilder>();
            var innerLogger = Mock.Of<ILogstashHttpLogger>();

            var logger = new LogstashLogger("", builder, innerLogger);

            Assert.NotNull(logger.Name);
        }

        [Fact]
        private void NameWhitespaceInitializesName()
        {
            var builder = Mock.Of<ILogMessageBuilder>();
            var innerLogger = Mock.Of<ILogstashHttpLogger>();

            var logger = new LogstashLogger("  ", builder, innerLogger);

            Assert.NotNull(logger.Name);
        }
    }
}
