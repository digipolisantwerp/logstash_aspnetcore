using System;
using Moq;
using Toolbox.Logstash.Message;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Message
{
    public class LogMessageBuilderTests
    {
        [Fact]
        private void ServiceProviderNullRaisesArgumentNullException()
        {
            var options = new LogstashOptions();
            var ex = Assert.Throws<ArgumentNullException>(() => new LogMessageBuilder(null, options));
            Assert.Equal("serviceProvider", ex.ParamName);
        }

        [Fact]
        private void OptionsNullRaisesArgumentNullException()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var ex = Assert.Throws<ArgumentNullException>(() => new LogMessageBuilder(serviceProvider, null));
            Assert.Equal("options", ex.ParamName);
        }

        [Fact]
        private void ServiceProviderIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var options = new LogstashOptions();

            var builder = new LogMessageBuilder(serviceProvider, options);

            Assert.Same(serviceProvider, builder.ServiceProvider);
        }

        [Fact]
        private void OptionsIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var options = new LogstashOptions();

            var builder = new LogMessageBuilder(serviceProvider, options);

            Assert.Same(options, builder.Options);
        }
    }
}
