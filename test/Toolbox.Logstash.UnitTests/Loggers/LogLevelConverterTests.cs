using System;
using Microsoft.Extensions.Logging;
using Toolbox.Logstash.Loggers;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Loggers
{
    public class LogLevelConverterTests
    {
        [Theory]
        [InlineData(LogLevel.Critical, LogStashLevel.Critical)]
        [InlineData(LogLevel.Debug, LogStashLevel.Debug)]
        [InlineData(LogLevel.Error, LogStashLevel.Error)]
        [InlineData(LogLevel.Information, LogStashLevel.Information)]
        [InlineData(LogLevel.None, LogStashLevel.None)]
        [InlineData(LogLevel.Verbose, LogStashLevel.Trace)]
        [InlineData(LogLevel.Warning, LogStashLevel.Warning)]
        private void ToLogstashLevelTests(LogLevel logLevel, LogStashLevel expectedLevel)
        {
            var level = LogLevelConverter.ToLogStashLevel(logLevel);
            Assert.Equal(expectedLevel, level);
        }
    }
}
