using System;
using Toolbox.Logstash.Message;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Message
{
    public class LogMessageCorrelationTests
    {
        [Fact]
        private void ApplicationIdIsSet()
        {
            var source = new LogMessageCorrelation("appid", "correlationid");
            Assert.Equal("appid", source.ApplicationId);
        }

        [Fact]
        private void CorrelationIdIsSet()
        {
            var source = new LogMessageCorrelation("appid", "correlationid");
            Assert.Equal("correlationid", source.CorrelationId);
        }
    }
}
