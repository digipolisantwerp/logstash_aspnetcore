using System;
using Toolbox.Logstash.Message;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Message
{
    public class LogMessageSourceTests
    {
        [Fact]
        private void ApplicationIdIsSet()
        {
            var source = new LogMessageSource("appid", "componentid");
            Assert.Equal("appid", source.ApplicationId);
        }

        [Fact]
        private void ComponentIdIsSet()
        {
            var source = new LogMessageSource("appid", "componentid");
            Assert.Equal("componentid", source.ComponentId);
        }
    }
}
