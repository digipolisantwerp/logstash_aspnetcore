using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.OptionsModel;
using Toolbox.Logstash.Client;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Startup
{
    public class AddLogstashLoggingOptionsTests
    {
        [Fact]
        private void SetupActionNullRaisesArgumentNullException()
        {
            Action<LogstashOptions> nullAction = null;
            var services = new ServiceCollection();

            var ex = Assert.Throws<ArgumentNullException>(() => services.AddLogstashLogging(nullAction));
            Assert.Equal("setupAction", ex.ParamName);
        }

        [Fact]
        private void LogstashOptionsIsRegisteredAsSingleton()
        {
            var services = new ServiceCollection();
            services.AddLogstashLogging(opt => opt.AppId = "MyApp");

            var registrations = services.Where(sd => sd.ServiceType == typeof(IConfigureOptions<LogstashOptions>)).ToArray();
            Assert.Equal(1, registrations.Count());
            Assert.Equal(ServiceLifetime.Singleton, registrations[0].Lifetime);

            var logstashOptions = registrations[0].ImplementationInstance as IConfigureOptions<LogstashOptions>;
            Assert.NotNull(logstashOptions);

            var options = new LogstashOptions();
            logstashOptions.Configure(options);
            Assert.Equal("MyApp", options.AppId);
        }

        [Fact]
        private void LogstashHttpLoggerProviderIsRegisteredAsSingleton()
        {
            var services = new ServiceCollection();
            services.AddLogstashLogging(opt => opt.AppId = "MyApp");

            var registrations = services.Where(sd => sd.ServiceType == typeof(LogstashHttpLoggerProvider)).ToArray();
            Assert.Equal(1, registrations.Count());
            Assert.Equal(ServiceLifetime.Singleton, registrations[0].Lifetime);
        }

        [Fact]
        private void DotNetWebClientProxyIsRegisteredAsTransient()
        {
            var services = new ServiceCollection();
            services.AddLogstashLogging(opt => opt.AppId = "MyApp");

            var registrations = services.Where(sd => sd.ServiceType == typeof(DotNetWebClientProxy)).ToArray();
            Assert.Equal(1, registrations.Count());
            Assert.Equal(ServiceLifetime.Transient, registrations[0].Lifetime);
        }
    }
}
