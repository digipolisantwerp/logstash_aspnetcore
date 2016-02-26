using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Extensions
{
    public class LoggerFactoryExensionsTests
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
    }
}
