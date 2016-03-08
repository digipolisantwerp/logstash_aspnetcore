using System;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Startup
{
    public class AddLogstashLoggingSetupActionTests
    {
        [Fact]
        private void AppNullRaisesArgumentNullException()
        {
            var factory = new LoggerFactory();
            var ex = Assert.Throws<ArgumentNullException>(() => factory.AddLogstashHttp(null, opt => opt.AppId = "MyApp"));
            Assert.Equal("app", ex.ParamName);
        }

        [Fact]
        private void SetupActionNullRaisesArgumentNullException()
        {
            var app = Mock.Of<IApplicationBuilder>();
            Action<LogstashOptions> nullAction = null;
            var factory = new LoggerFactory();
            var ex = Assert.Throws<ArgumentNullException>(() => factory.AddLogstashHttp(app, nullAction));
            Assert.Equal("setupAction", ex.ParamName);
        }
    }
}
