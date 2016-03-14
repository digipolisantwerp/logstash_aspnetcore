using System;
using Toolbox.Logstash.Client;
using Xunit;

namespace Toolbox.Logstash.UnitTests.Client
{
    public class DotNetWebClientProxyCtorTests
    {
        [Fact]
        private void UriStringNullRaisesArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new DotNetWebClientProxy(null, "useragent"));
            Assert.Equal("uriString", ex.ParamName);
        }

        [Fact]
        private void UriStringEmptyRaisesUriFormatException()
        {
            Assert.Throws<UriFormatException>(() => new DotNetWebClientProxy("", "useragent"));
        }

        [Fact]
        private void UriStringWhitespaceRaisesUriFormatException()
        {
            Assert.Throws<UriFormatException>(() => new DotNetWebClientProxy("  ", "useragent"));
        }

        [Fact]
        private void UriStringInvalidRaisesUriFormatException()
        {
            Assert.Throws<UriFormatException>(() => new DotNetWebClientProxy("abc", "useragent"));
        }

        [Fact]
        private void UriStringValidSetsUri()
        {
            var client = new DotNetWebClientProxy("http://localhost", "useragent");
            Assert.Equal("http://localhost/", client.Uri.ToString());
        }

        [Fact]
        private void UserAgentNullRaisesArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => new DotNetWebClientProxy("http://localhost", null));
            Assert.Equal("userAgent", ex.ParamName);
        }

        [Fact]
        private void UserAgentEmptyRaisesArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => new DotNetWebClientProxy("http://localhost", ""));
            Assert.Equal("userAgent", ex.ParamName);
        }

        [Fact]
        private void UserAgentWhitespaceRaisesArgumentException()
        {
            var ex = Assert.Throws<ArgumentException>(() => new DotNetWebClientProxy("http://localhost", "   "));
            Assert.Equal("userAgent", ex.ParamName);
        }
    }
}
