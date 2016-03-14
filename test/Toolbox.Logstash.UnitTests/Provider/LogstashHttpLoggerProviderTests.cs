using System;
using Moq;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Provider
{
    public class LogstashHttpLoggerProviderTests
    {
        [Fact]
        private void OptionsNullRaisesArgumentNullException()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            LogstashOptions nullOptions = null;
            var ex = Assert.Throws<ArgumentNullException>(() => new LogstashHttpLoggerProvider(serviceProvider, nullOptions));
            Assert.Equal("options", ex.ParamName);
        }

        [Fact]
        private void ServiceProviderNullRaisesArgumentNullException()
        {
            IServiceProvider nullProvider = null;
            var options = new LogstashOptions();
            var ex = Assert.Throws<ArgumentNullException>(() => new LogstashHttpLoggerProvider(nullProvider, options));
            Assert.Equal("serviceProvider", ex.ParamName);
        }

        [Fact]
        private void OptionsIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var options = new LogstashOptions();
            var provider = new LogstashHttpLoggerProvider(serviceProvider, options);
            Assert.Same(options, provider.Options);
        }

        [Fact]
        private void ServiceProviderIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var options = new LogstashOptions();
            var provider = new LogstashHttpLoggerProvider(serviceProvider, options);
            Assert.Same(serviceProvider, provider.ServiceProvider);
        }
    }
}
