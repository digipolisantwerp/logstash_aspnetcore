using System;
using Toolbox.Logstash.Loggers;
using Toolbox.Logstash.Client;
using Xunit;
using Microsoft.Extensions.OptionsModel;
using Moq;

namespace Toolbox.Logstash.UnitTests.Loggers
{
    public class LogstashHttpLoggerTests
    {
        [Fact]
        private void OptionsNullRaisesArgumentNullException()
        {
            IOptions<LogstashOptions> nullOptions = null;
            var webClient = new DotNetWebClientProxy();
            var ex = Assert.Throws<ArgumentNullException>(() => new LogstashHttpLogger(nullOptions, webClient));
            Assert.Equal("options", ex.ParamName);
        }

        [Fact]
        private void WebClientNullRaisesArgumentNullException()
        {
            var options = Mock.Of<IOptions<LogstashOptions>>();
            IWebClient nullClient = null;
            var ex = Assert.Throws<ArgumentNullException>(() => new LogstashHttpLogger(options, nullClient));
            Assert.Equal("webClient", ex.ParamName);
        }

        [Fact]
        private void OptionsIsSet()
        {
            var options = Mock.Of<IOptions<LogstashOptions>>();
            var webClient = new DotNetWebClientProxy();

            var logger = new LogstashHttpLogger(options, webClient);

            Assert.Same(options, logger.Options);
        }

        [Fact]
        private void WebClientIsSet()
        {
            var options = Mock.Of<IOptions<LogstashOptions>>();
            var webClient = new DotNetWebClientProxy();

            var logger = new LogstashHttpLogger(options, webClient);

            Assert.Same(webClient, logger.WebClient);
        }
    }
}
