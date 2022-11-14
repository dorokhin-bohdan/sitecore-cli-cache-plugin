﻿using FluentAssertions;
using Moq;
using Sitecore.Collections;
using Sitecore.DevEx.Extensibility.Cache.Api.Services;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners;
using Sitecore.DevEx.Extensibility.Cache.Api.Tests.Fakes;
using Sitecore.DevEx.Extensibility.Cache.Api.Tests.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.Sites;
using Sitecore.Web;
using Xunit;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Tests.Services.CacheCleaners;

public class XslCacheCleanerTests : BaseCacheCleanerTests<XslCacheCleaner>
{
    protected override CacheType ExpectedCacheType => CacheType.Xsl;
    protected override Mock<XslCacheCleaner> CacheCleanerMock { get; }

    public XslCacheCleanerTests()
    {
        var bytesConverter = new Mock<IBytesConverter>();
        CacheCleanerMock = new Mock<XslCacheCleaner>(bytesConverter.Object)
        {
            CallBase = true
        };
    }
        
    [Fact]
    public void GetCacheInfo_ShouldCallProperCache()
    {
        // Arrange
        var siteContextMock = new Mock<FakeSiteContext>(SiteInfo.Create(new StringDictionary()))
            { CallBase = true };
        var cachesMock = new Mock<SiteCaches>(siteContextMock.Object) { CallBase = true };
        siteContextMock.SetupGet(x => x.Caches).Returns(cachesMock.Object);

        // Act
        var result = CacheCleanerMock.Object.GetCacheInfo(siteContextMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(cachesMock.Object.XslCache.InnerCache);
    }
}