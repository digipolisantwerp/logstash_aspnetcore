using System;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Startup
{
    public class LoggerFactoryExtensionsTests
    {
        [Fact]
        private void SetupActionNullRaisesArgumentNullException()
        {
            Action<LogstashOptions> nullAction = null;
            var factory = new LoggerFactory();
            var ex = Assert.Throws<ArgumentNullException>(() => factory.AddLogstashHttp(nullAction));
            Assert.Equal("setupAction", ex.ParamName);
        }

        [Fact]
        private void ConfigNullRaisesArgumentNullException()
        {
            IConfiguration nullConfig = null;
            var factory = new LoggerFactory();
            var ex = Assert.Throws<ArgumentNullException>(() => factory.AddLogstashHttp(nullConfig));
            Assert.Equal("config", ex.ParamName);
        }

        [Fact]
        private void AppNullRaisesArgumentNullException()
        {
            IApplicationBuilder nullApp = null;
            var factory = new LoggerFactory();
            var ex = Assert.Throws<ArgumentNullException>(() => factory.AddLogstashHttp(nullApp));
            Assert.Equal("app", ex.ParamName);
        }
    }
}
