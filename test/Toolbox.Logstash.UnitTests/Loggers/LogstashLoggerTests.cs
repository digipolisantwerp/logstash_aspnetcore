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
        private void ServiceProviderNullRaisesArgumentNullException()
        {
            var builder = Mock.Of<ILogMessageBuilder>();
            var options = new LogstashOptions();
            var logger = Mock.Of<ILogstashHttpLogger>();
            var ex = Assert.Throws<ArgumentNullException>(() => new LogstashLogger(null, builder, options, logger));
            Assert.Equal("serviceProvider", ex.ParamName);
        }

        [Fact]
        private void BuilderNullRaisesArgumentNullException()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var options = new LogstashOptions();
            var logger = Mock.Of<ILogstashHttpLogger>();
            var ex = Assert.Throws<ArgumentNullException>(() => new LogstashLogger(serviceProvider, null, options, logger));
            Assert.Equal("logMessageBuilder", ex.ParamName);
        }

        [Fact]
        private void OptionsNullRaisesArgumentNullException()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var builder = Mock.Of<ILogMessageBuilder>();
            var logger = Mock.Of<ILogstashHttpLogger>();
            var ex = Assert.Throws<ArgumentNullException>(() => new LogstashLogger(serviceProvider, builder, null, logger));
            Assert.Equal("options", ex.ParamName);
        }


        [Fact]
        private void LoggerNullRaisesArgumentNullException()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var builder = Mock.Of<ILogMessageBuilder>();
            var options = new LogstashOptions();
            var ex = Assert.Throws<ArgumentNullException>(() => new LogstashLogger(serviceProvider, builder, options, null));
            Assert.Equal("logger", ex.ParamName);
        }


        [Fact]
        private void ServiceProviderIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var builder = Mock.Of<ILogMessageBuilder>();
            var options = new LogstashOptions();
            var innerLogger = Mock.Of<ILogstashHttpLogger>();

            var logger = new LogstashLogger(serviceProvider, builder, options, innerLogger);

            Assert.Same(serviceProvider, logger.ServiceProvider);
        }

        [Fact]
        private void LogMessageBuilderIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var builder = Mock.Of<ILogMessageBuilder>();
            var options = new LogstashOptions();
            var innerLogger = Mock.Of<ILogstashHttpLogger>();

            var logger = new LogstashLogger(serviceProvider, builder, options, innerLogger);

            Assert.Same(builder, logger.LogMessageBuilder);
        }

        [Fact]
        private void OptionsIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var builder = Mock.Of<ILogMessageBuilder>();
            var options = new LogstashOptions();
            var innerLogger = Mock.Of<ILogstashHttpLogger>();

            var logger = new LogstashLogger(serviceProvider, builder, options, innerLogger);

            Assert.Same(options, logger.Options);
        }

        [Fact]
        private void LoggerIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var builder = Mock.Of<ILogMessageBuilder>();
            var options = new LogstashOptions();
            var innerLogger = Mock.Of<ILogstashHttpLogger>();

            var logger = new LogstashLogger(serviceProvider, builder, options, innerLogger);

            Assert.Same(innerLogger, logger.Logger);
        }
    }
}
