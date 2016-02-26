using System;
using Toolbox.Logstash.Options.Internal;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Exceptions
{
    public class InvalidOptionExceptionTests
    {
        [Fact]
        private void OptionKeyIsSet()
        {
            var ex = new InvalidOptionException("key", "value");
            Assert.Equal("key", ex.OptionKey);
        }

        [Fact]
        private void OptionValueIsSet()
        {
            var ex = new InvalidOptionException("key", "value");
            Assert.Equal("value", ex.OptionValue);
        }

        [Fact]
        private void DefaultMessageIsSetInDefaultCtor()
        {
            var ex = new InvalidOptionException();
            Assert.Equal(Defaults.Exceptions.InvalidOptionException.Message, ex.Message);
        }

        [Fact]
        private void DefaultMessageIsSet()
        {
            var ex = new InvalidOptionException("aKey", "aValue");
            Assert.Equal(Defaults.Exceptions.InvalidOptionException.Message, ex.Message);
        }

        [Fact]
        private void MessageIsSet()
        {
            var ex = new InvalidOptionException("aKey", "aValue", "aMessage");
            Assert.Equal("aMessage", ex.Message);
        }
    }
}
