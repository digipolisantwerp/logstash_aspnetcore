using System;
using Moq;
using Toolbox.Logstash.Loggers;
using Toolbox.Logstash.Message;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Message
{
    public class LogMessageBuilderCtorTests
    {
        [Fact]
        private void ServiceProviderNullRaisesArgumentNullException()
        {
            var converter = new LogLevelConverter();
            var options = new LogstashOptions();
            var ex = Assert.Throws<ArgumentNullException>(() => new LogMessageBuilder(null, converter, options));
            Assert.Equal("serviceProvider", ex.ParamName);
        }

        [Fact]
        private void LogLevelConverterNullRaisesArgumentNullException()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var options = new LogstashOptions();
            var ex = Assert.Throws<ArgumentNullException>(() => new LogMessageBuilder(serviceProvider, null, options));
            Assert.Equal("logLevelConverter", ex.ParamName);
        }

        [Fact]
        private void OptionsNullRaisesArgumentNullException()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var ex = Assert.Throws<ArgumentNullException>(() => new LogMessageBuilder(serviceProvider, converter, null));
            Assert.Equal("options", ex.ParamName);
        }

        [Fact]
        private void ServiceProviderIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions();

            var builder = new LogMessageBuilder(serviceProvider, converter, options);

            Assert.Same(serviceProvider, builder.ServiceProvider);
        }

        [Fact]
        private void OptionsIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions();

            var builder = new LogMessageBuilder(serviceProvider, converter, options);

            Assert.Same(options, builder.Options);
        }

        [Fact]
        private void LogLevelConverterIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions();

            var builder = new LogMessageBuilder(serviceProvider, converter, options);

            Assert.Same(converter, builder.LogLevelConverter);
        }

        [Fact]
        private void LocalIPAddressIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions();

            var builder = new LogMessageBuilder(serviceProvider, converter, options);

            Assert.NotNull(builder.LocalIPAddress);
        }

        [Fact]
        private void CurrentProcessIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions();

            var builder = new LogMessageBuilder(serviceProvider, converter, options);

            Assert.NotNull(builder.CurrentProcess);
        }
    }
}
