using System;
using Toolbox.Logstash.Options.Internal;
using Toolbox.Logstash.Message;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Message
{
    public class LogMessageTests
    {
        [Fact]
        private void HeaderIsInitialized()
        {
            var message = new LogMessage();
            Assert.NotNull(message.Header);
        }

        [Fact]
        private void BodyIsInitialized()
        {
            var message = new LogMessage();
            Assert.NotNull(message.Body);
        }

        [Fact]
        private void DefaultLevelIsSet()
        {
            var message = new LogMessage();
            Assert.Equal(Defaults.Message.Level, message.Body.Level);
        }

        [Fact]
        private void LevelIsSet()
        {
            var message = new LogMessage(LogStashLevel.Trace);
            Assert.Equal(LogStashLevel.Trace, message.Body.Level);
        }
    }
}
