using System;
using Toolbox.Logstash.Loggers;
using Toolbox.Logstash.Client;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Loggers
{
    public class LogstashHttpLoggerTests
    {
        [Fact]
        private void OptionsNullRaisesArgumentNullException()
        {
            LogstashOptions nullOptions = null;
            var webClient = new DotNetWebClientProxy();
            var ex = Assert.Throws<ArgumentNullException>(() => new LogstashHttpLogger(nullOptions, webClient));
            Assert.Equal("options", ex.ParamName);
        }

        [Fact]
        private void WebClientNullRaisesArgumentNullException()
        {
            var options = new LogstashOptions();
            IWebClient nullClient = null;
            var ex = Assert.Throws<ArgumentNullException>(() => new LogstashHttpLogger(options, nullClient));
            Assert.Equal("webClient", ex.ParamName);
        }

        [Fact]
        private void OptionsIsSet()
        {
            var options = new LogstashOptions();
            var webClient = new DotNetWebClientProxy();

            var logger = new LogstashHttpLogger(options, webClient);

            Assert.Same(options, logger.Options);
        }

        [Fact]
        private void WebClientIsSet()
        {
            var options = new LogstashOptions();
            var webClient = new DotNetWebClientProxy();

            var logger = new LogstashHttpLogger(options, webClient);

            Assert.Same(webClient, logger.WebClient);
        }
    }
}
