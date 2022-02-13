using System.Globalization;
using FluentAssertions;
using Sitecore.DevEx.Extensibility.Cache.Api.Services;
using Xunit;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Tests.Services
{
    public class BytesConverterTests
    {
        private readonly IBytesConverter _bytesConverter;

        public BytesConverterTests()
        {
            _bytesConverter = new BytesConverter();
        }

        [Theory]
        [InlineData(0, 2, 0, "bytes")]
        [InlineData(123, 2, 123, "bytes")]
        [InlineData(1024, 2, 1, "KB")]
        [InlineData(1024*1024, 2, 1, "MB")]
        [InlineData(1024*1024*23 + 1024*800, 2, 23.78, "MB")]
        [InlineData(int.MaxValue, 2, 2, "GB")]
        [InlineData(long.MaxValue, 2, 8, "EB")]
        public void ToReadable_BytesToReadableString_ShouldBeProperUnitAndSize(long bytes, int decimals,
            decimal expectedSize,
            string expectedUnit)
        {
            // Act
            var result = _bytesConverter.ToReadable(bytes, decimals);
            var expectedSubstring = $"{expectedSize.ToString("N" + decimals, CultureInfo.InvariantCulture)} {expectedUnit}";

            // Assert
            result.Should().ContainEquivalentOf(expectedSubstring);
        }
    }
}