using System;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Startup
{
    public class AddLogstashLoggingConfigTests
    {
        [Fact]
        private void AppNullRaisesArgumentNullException()
        {
            var config = Mock.Of<IConfiguration>();
            var factory = new LoggerFactory();
            var ex = Assert.Throws<ArgumentNullException>(() => factory.AddLogstashHttp(null, config));
            Assert.Equal("app", ex.ParamName);
        }

        [Fact]
        private void ConfigNullRaisesArgumentNullException()
        {
            var app = Mock.Of<IApplicationBuilder>();
            IConfiguration nullConfig = null;
            var factory = new LoggerFactory();
            var ex = Assert.Throws<ArgumentNullException>(() => factory.AddLogstashHttp(app, nullConfig));
            Assert.Equal("config", ex.ParamName);
        }
    }
}
