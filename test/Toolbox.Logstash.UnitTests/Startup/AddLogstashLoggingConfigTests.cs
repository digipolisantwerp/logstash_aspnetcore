using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
using Toolbox.Logstash.Client;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Startup
{
    public class AddLogstashLoggingConfigTests
    {
        [Fact]
        private void ConfigNullRaisesArgumentNullException()
        {
            IConfiguration nullConfig = null;
            var services = new ServiceCollection();

            var ex = Assert.Throws<ArgumentNullException>(() => services.AddLogstashLogging(nullConfig));

            Assert.Equal("config", ex.ParamName);
        }

        [Fact]
        private void LogstashOptionsIsRegisteredAsSingleton()
        {
            var config = TestOptionsFactory.CreateMemoryConfig("myApp", "anUrl", "myIndex", LogLevel.Information);

            var services = new ServiceCollection();
            services.AddLogstashLogging(config);

            var registrations = services.Where(sd => sd.ServiceType == typeof(IConfigureOptions<LogstashOptions>)).ToArray();
            Assert.Equal(1, registrations.Count());
            Assert.Equal(ServiceLifetime.Singleton, registrations[0].Lifetime);

            var logstashOptions = registrations[0].ImplementationInstance as IConfigureOptions<LogstashOptions>;
            Assert.NotNull(logstashOptions);

            var expectedOptions = new LogstashOptions();
            logstashOptions.Configure(expectedOptions);
            Assert.Equal("myApp", expectedOptions.AppId);
            Assert.Equal("myIndex", expectedOptions.Index);
        }

        [Fact]
        private void LogstashHttpLoggerProviderIsRegisteredAsSingleton()
        {
            var config = TestOptionsFactory.CreateMemoryConfig("MyApp", "MyUrl", "MyIndex");

            var services = new ServiceCollection();
            services.AddLogstashLogging(config);

            var registrations = services.Where(sd => sd.ServiceType == typeof(LogstashHttpLoggerProvider)).ToArray();
            Assert.Equal(1, registrations.Count());
            Assert.Equal(ServiceLifetime.Singleton, registrations[0].Lifetime);
        }

        [Fact]
        private void DotNetWebClientProxyIsRegisteredAsTransient()
        {
            var config = TestOptionsFactory.CreateMemoryConfig("MyApp", "MyUrl", "MyIndex");

            var services = new ServiceCollection();
            services.AddLogstashLogging(config);

            var registrations = services.Where(sd => sd.ServiceType == typeof(DotNetWebClientProxy)).ToArray();
            Assert.Equal(1, registrations.Count());
            Assert.Equal(ServiceLifetime.Transient, registrations[0].Lifetime);
        }
    }
}
