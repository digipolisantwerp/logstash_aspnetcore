using System;
using Toolbox.Logstash.Options.Internal;
using Toolbox.Logstash.Message;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Message
{
    public class LogMessageBodyTests
    {
        [Fact]
        private void DefaultLevelIsSet()
        {
            var body = new LogMessageBody();
            Assert.Equal(Defaults.Message.Level, body.Level);
        }

        [Fact]
        private void LevelIsSet()
        {
            var body = new LogMessageBody(LogstashLevel.Error);
            Assert.Equal(LogstashLevel.Error, body.Level);
        }
    }
}
