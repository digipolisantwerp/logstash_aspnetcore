using System;
using Microsoft.Extensions.Logging;
using Moq;
using Toolbox.Correlation;
using Toolbox.Logstash.Loggers;
using Toolbox.Logstash.Message;
using Toolbox.Logstash.Options.Internal;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Message
{
    public class LogMessageBuilderBuildTests
    {
        [Fact]
        private void StateAndExceptionNullReturnsNull()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions();
            var builder = new LogMessageBuilder(serviceProvider, converter, options);

            var message = builder.Build("myLogger", LogLevel.Information, null, null);
        }

        [Fact]
        private void LevelIsConverted()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new Mock<ILogLevelConverter>();
            var options = new LogstashOptions();

            var level = LogLevel.Information;

            converter.Setup((c) => c.ToLogStashLevel(level)).Returns(LogstashLevel.Information).Verifiable();

            var builder = new LogMessageBuilder(serviceProvider, converter.Object, options);

            var message = builder.Build("myLogger", LogLevel.Information, "state", null);

            converter.Verify();
        }

        [Fact]
        private void FormatterIsCalled()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions();

            var isCalled = false;

            var builder = new LogMessageBuilder(serviceProvider, converter, options);

            var message = builder.Build("myLogger", LogLevel.Information, "state", null, (state, ex) => { isCalled = true; return "called"; });

            Assert.True(isCalled);
        }

        [Fact]
        private void IndexIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions() { AppId = "myApp", Index = "myIndex", Url = "http://localhost" };

            var builder = new LogMessageBuilder(serviceProvider, converter, options);

            var message = builder.Build("myLogger", LogLevel.Information, "state", null);

            Assert.Equal(options.Index, message.Header.Index);
        }

        [Fact]
        private void ProcessIdIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions() { AppId = "myApp", Index = "myIndex", Url = "http://localhost" };

            var builder = new LogMessageBuilder(serviceProvider, converter, options);

            var message = builder.Build("myLogger", LogLevel.Information, "state", null);

            Assert.Equal(builder.CurrentProcess, message.Header.ProcessId);
        }

        [Fact]
        private void IPAddressIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions() { AppId = "myApp", Index = "myIndex", Url = "http://localhost" };

            var builder = new LogMessageBuilder(serviceProvider, converter, options);

            var message = builder.Build("myLogger", LogLevel.Information, "state", null);

            Assert.Equal(builder.LocalIPAddress, message.Header.IPAddress);
        }

        [Fact]
        private void ThreadIdIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions() { AppId = "myApp", Index = "myIndex", Url = "http://localhost" };

            var builder = new LogMessageBuilder(serviceProvider, converter, options);

            var message = builder.Build("myLogger", LogLevel.Information, "state", null);

            Assert.NotNull(message.Header.ThreadId);
        }

        [Fact]
        private void TimestampIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions() { AppId = "myApp", Index = "myIndex", Url = "http://localhost" };

            var builder = new LogMessageBuilder(serviceProvider, converter, options);

            var message = builder.Build("myLogger", LogLevel.Information, "state", null);

            var now = DateTime.Now;
            Assert.InRange(message.Header.TimeStamp, now.AddMinutes(-1), now.AddMinutes(1));
        }

        [Fact]
        private void HeaderVersionIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions() { AppId = "myApp", Index = "myIndex", Url = "http://localhost" };

            var builder = new LogMessageBuilder(serviceProvider, converter, options);

            var message = builder.Build("myLogger", LogLevel.Information, "state", null);

            Assert.Equal(Defaults.Message.HeaderVersion, message.Header.VersionNumber);
        }

        [Fact]
        private void MessageVersionIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions() { AppId = "myApp", Index = "myIndex", Url = "http://localhost" };

            var builder = new LogMessageBuilder(serviceProvider, converter, options);

            var message = builder.Build("myLogger", LogLevel.Information, "state", null);

            Assert.Equal(options.MessageVersion, message.Body.VersionNumber);
        }

        [Fact]
        private void ApplicationIdIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions() { AppId = "myApp", Index = "myIndex", Url = "http://localhost" };

            var builder = new LogMessageBuilder(serviceProvider, converter, options);

            var message = builder.Build("myLogger", LogLevel.Information, "state", null);

            Assert.Equal(options.AppId, message.Header.Source.ApplicationId);
        }
         
        [Fact]
        private void ComponentIdIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions() { AppId = "myApp", Index = "myIndex", Url = "http://localhost" };

            var builder = new LogMessageBuilder(serviceProvider, converter, options);

            var message = builder.Build("myLogger", LogLevel.Information, "state", null);

            Assert.Equal("myLogger", message.Header.Source.ComponentId);
        }

        [Fact]
        private void StateIsSet()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions() { AppId = "myApp", Index = "myIndex", Url = "http://localhost" };

            var builder = new LogMessageBuilder(serviceProvider, converter, options);

            var message = builder.Build("myLogger", LogLevel.Information, "state", null);

            Assert.Equal("state", message.Body.Content);
        }

        [Fact]
        private void CorrelationApplicationIdIsSetFromRegisteredCorrelationContext()
        {
            var correlationContext = new Mock<ICorrelationContext>();
            var serviceProvider = new Mock<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions() { AppId = "myApp", Index = "myIndex", Url = "http://localhost" };

            correlationContext.SetupGet(p => p.CorrelationSource).Returns("correlation-app");
            serviceProvider.Setup(sp => sp.GetService(typeof(ICorrelationContext))).Returns(correlationContext.Object);

            var builder = new LogMessageBuilder(serviceProvider.Object, converter, options);

            var message = builder.Build("myLogger", LogLevel.Information, "state", null);

            Assert.Equal("correlation-app", message.Header.Correlation.ApplicationId);
        }

        [Fact]
        private void CorrelationIdIsSetFromRegisteredCorrelationContext()
        {
            var correlationContext = new Mock<ICorrelationContext>();
            var serviceProvider = new Mock<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions() { AppId = "myApp", Index = "myIndex", Url = "http://localhost" };

            correlationContext.SetupGet(p => p.CorrelationId).Returns("correlation-id");
            serviceProvider.Setup(sp => sp.GetService(typeof(ICorrelationContext))).Returns(correlationContext.Object);

            var builder = new LogMessageBuilder(serviceProvider.Object, converter, options);

            var message = builder.Build("myLogger", LogLevel.Information, "state", null);

            Assert.Equal("correlation-id", message.Header.Correlation.CorrelationId);
        }

        [Fact]
        private void CorrelationApplicationIdIsSetWithoutRegisteredCorrelationContext()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions() { AppId = "myApp", Index = "myIndex", Url = "http://localhost" };

            var builder = new LogMessageBuilder(serviceProvider, converter, options);

            var message = builder.Build("myLogger", LogLevel.Information, "state", null);

            Assert.Equal(options.AppId, message.Header.Correlation.ApplicationId);
        }

        [Fact]
        private void CorrelationIdIsSetWithoutRegisteredCorrelationContext()
        {
            var serviceProvider = Mock.Of<IServiceProvider>();
            var converter = new LogLevelConverter();
            var options = new LogstashOptions() { AppId = "myApp", Index = "myIndex", Url = "http://localhost" };

            var builder = new LogMessageBuilder(serviceProvider, converter, options);

            var message = builder.Build("myLogger", LogLevel.Information, "state", null);

            Assert.NotNull(message.Header.Correlation.CorrelationId);
        }
    }
}
