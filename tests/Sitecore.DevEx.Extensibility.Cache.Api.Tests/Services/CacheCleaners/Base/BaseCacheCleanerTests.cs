using System;
using FluentAssertions;
using Moq;
using Sitecore.Caching;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.Sites;
using Xunit;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Tests.Services.CacheCleaners.Base;

public abstract class BaseCacheCleanerTests<TCacheCleaner> where TCacheCleaner : class, ICacheCleaner
{
    protected abstract CacheType ExpectedCacheType { get; }
    protected abstract Mock<TCacheCleaner> CacheCleanerMock { get; }

    [Fact]
    public void CacheType_ShouldBeExpected()
    {
        // Assert
        CacheCleanerMock.Object.CacheType.Should().Be(ExpectedCacheType);
    }

    [Fact]
    public void Clear_CacheInfoClear_ShouldBeCalled()
    {
        // Arrange
        var cacheMock = new Mock<ICacheInfo>();

        CacheCleanerMock
            .Setup(m => m.GetCacheInfo(It.IsAny<SiteContext>()))
            .Returns(cacheMock.Object);

        // Act
        _ = CacheCleanerMock.Object.Clear(It.IsAny<SiteContext>());

        // Assert
        cacheMock.Verify(c => c.Clear());
    }

    [Fact]
    public void Clear_CacheClearOk_ShouldBeSuccessTrue()
    {
        // Arrange
        var cacheMock = new Mock<ICacheInfo>();
        cacheMock.SetupGet(c => c.Size).Returns(100);

        CacheCleanerMock
            .Setup(m => m.GetCacheInfo(It.IsAny<SiteContext>()))
            .Returns(cacheMock.Object);

        // Act
        var result = CacheCleanerMock.Object.Clear(It.IsAny<SiteContext>());

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Messages.Should().NotBeEmpty();
    }

    [Fact]
    public void Clear_CacheClearError_ShouldBeSuccessFalse()
    {
        // Arrange
        var cacheMock = new Mock<ICacheInfo>();
        cacheMock.SetupGet(c => c.Size).Returns(100);
        cacheMock.Setup(c => c.Clear()).Throws<ArgumentException>();

        CacheCleanerMock
            .Setup(m => m.GetCacheInfo(It.IsAny<SiteContext>()))
            .Returns(cacheMock.Object);

        // Act
        var result = CacheCleanerMock.Object.Clear(It.IsAny<SiteContext>());

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Messages.Should().NotBeEmpty();
    }
}