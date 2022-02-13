using System.Linq;
using FluentAssertions;
using Sitecore.DevEx.Extensibility.Cache.Api.Services;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Xunit;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Tests.Services
{
    public class EnumServiceTests
    {
        private readonly IEnumService _enumService;

        public EnumServiceTests()
        {
            _enumService = new EnumService();
        }

        [Theory]
        [InlineData(CacheType.Data | CacheType.Html, new[] { CacheType.Data, CacheType.Html })]
        [InlineData(CacheType.Data | CacheType.Html | CacheType.Item, new[] { CacheType.Data, CacheType.Html, CacheType.Item })]
        public void GetFlagValues_CombinedValue_ShouldReturnAllEnums(CacheType combinedValue,
            CacheType[] expectedCacheTypes)
        {
            // Act
            var result = _enumService.GetFlagValues(combinedValue).ToList();

            // Assert
            result.Should().BeEquivalentTo(expectedCacheTypes);
        }
    }
}