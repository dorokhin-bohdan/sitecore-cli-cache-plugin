using FluentAssertions;
using Moq;
using Moq.Protected;
using Sitecore.Caching;
using Sitecore.Caching.Generics;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.DevEx.Extensibility.Cache.Api.Services;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners;
using Sitecore.DevEx.Extensibility.Cache.Api.Tests.Fakes;
using Sitecore.DevEx.Extensibility.Cache.Api.Tests.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.Web;
using Xunit;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Tests.Services.CacheCleaners;

public class IsFallbackValidCacheCleanerTests : BaseCacheCleanerTests<IsFallbackValidCacheCleaner>
{
    protected override CacheType ExpectedCacheType => CacheType.IsFallbackValid;
    protected override Mock<IsFallbackValidCacheCleaner> CacheCleanerMock { get; }

    public IsFallbackValidCacheCleanerTests()
    {
        var bytesConverter = new Mock<IBytesConverter>();
        CacheCleanerMock = new Mock<IsFallbackValidCacheCleaner>(bytesConverter.Object)
        {
            CallBase = true
        };
    }
        
    [Fact]
    public void GetCacheInfo_ShouldCallProperCache()
    {
        // Arrange
        var cacheMock = new Mock<ICache<IsLanguageFallbackValidCacheKey>>();
        cacheMock.SetupGet(x => x.Name).Returns("FakeCache");
            
        var dbMock = new Mock<Database> { CallBase = true };
        var dbCachesMock = new Mock<DatabaseCaches>(dbMock.Object) { CallBase = true };
        var fallbackCacheMock = new Mock<IsLanguageFallbackValidCache>(dbMock.Object, 100);
        fallbackCacheMock
            .Protected()
            .Setup<ICache<IsLanguageFallbackValidCacheKey>>("GetIndexedCache")
            .Returns(cacheMock.Object);
                
        var siteContextMock = new Mock<FakeSiteContext>(SiteInfo.Create(new StringDictionary()), dbMock.Object)
            { CallBase = true };

        dbCachesMock.SetupGet(x => x.IsFallbackValidCache).Returns(fallbackCacheMock.Object);
        dbMock.SetupGet(x => x.Caches).Returns(dbCachesMock.Object);

        // Act
        var result = CacheCleanerMock.Object.GetCacheInfo(siteContextMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(cacheMock.Object);
    }
}