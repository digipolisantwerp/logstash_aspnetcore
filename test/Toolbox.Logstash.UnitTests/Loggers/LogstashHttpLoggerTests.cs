using System;
using Toolbox.Logstash.Client;
using Toolbox.Logstash.Loggers;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Loggers
{
    public class LogstashHttpLoggerTests
    {
        [Fact]
        private void WebClientNullRaisesArgumentNullException()
        {
            IWebClient nullClient = null;
            var ex = Assert.Throws<ArgumentNullException>(() => new LogstashHttpLogger(nullClient));
            Assert.Equal("webClient", ex.ParamName);
        }

        [Fact]
        private void WebClientIsSet()
        {
            var webClient = new DotNetWebClientProxy();
            var logger = new LogstashHttpLogger(webClient);
            Assert.Same(webClient, logger.WebClient);
        }
    }
}
