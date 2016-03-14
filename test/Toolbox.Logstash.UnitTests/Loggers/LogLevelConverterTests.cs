using System;
using Microsoft.Extensions.Logging;
using Toolbox.Logstash.Loggers;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Loggers
{
    public class LogLevelConverterTests
    {
        [Theory]
        [InlineData(LogLevel.Critical, LogstashLevel.Critical)]
        [InlineData(LogLevel.Debug, LogstashLevel.Debug)]
        [InlineData(LogLevel.Error, LogstashLevel.Error)]
        [InlineData(LogLevel.Information, LogstashLevel.Information)]
        [InlineData(LogLevel.None, LogstashLevel.None)]
        [InlineData(LogLevel.Verbose, LogstashLevel.Trace)]
        [InlineData(LogLevel.Warning, LogstashLevel.Warning)]
        private void ToLogstashLevelTests(LogLevel logLevel, LogstashLevel expectedLevel)
        {
            var converter = new LogLevelConverter();
            var level = converter.ToLogStashLevel(logLevel);
            Assert.Equal(expectedLevel, level);
        }
    }
}
