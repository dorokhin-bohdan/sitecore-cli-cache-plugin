using FluentAssertions;
using Moq;
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

public class DataCacheCleanerTests : BaseCacheCleanerTests<DataCacheCleaner>
{
    protected override CacheType ExpectedCacheType => CacheType.Data;
    protected override Mock<DataCacheCleaner> CacheCleanerMock { get; }

    public DataCacheCleanerTests()
    {
        var bytesConverter = new Mock<IBytesConverter>();

        CacheCleanerMock = new Mock<DataCacheCleaner>(bytesConverter.Object)
        {
            CallBase = true
        };
    }

    [Fact]
    public void GetCacheInfo_ShouldCallProperCache()
    {
        // Arrange
        var cacheMock = new Mock<ICache<ID>>();
        var dbMock = new Mock<Database> { CallBase = true };
        var dbCachesMock = new Mock<DatabaseCaches>(dbMock.Object) { CallBase = true };
        var dataCacheMock = new Mock<FakeDataCache>(dbMock.Object, 100, cacheMock.Object) { CallBase = true };
        var siteContextMock = new Mock<FakeSiteContext>(SiteInfo.Create(new StringDictionary()), dbMock.Object)
            { CallBase = true };

        dbCachesMock.SetupGet(x => x.DataCache).Returns(dataCacheMock.Object);
        dbMock.SetupGet(x => x.Caches).Returns(dbCachesMock.Object);

        // Act
        var result = CacheCleanerMock.Object.GetCacheInfo(siteContextMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(cacheMock.Object);
    }
}