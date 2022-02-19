using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Sitecore.Abstractions;
using Sitecore.Caching;
using Sitecore.Collections;
using Sitecore.DevEx.Extensibility.Cache.Api.Services;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;
using Sitecore.Web;
using Xunit;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Tests.Services
{
    public class CacheServiceTests
    {
        private readonly Mock<BaseCacheManager> _baseCacheManagerMock;
        private readonly Mock<BaseSiteContextFactory> _baseSiteContextFactoryMock;
        private readonly List<ICacheCleaner> _cleaners;

        private readonly ICacheService _cacheService;

        public CacheServiceTests()
        {
            _baseCacheManagerMock = new Mock<BaseCacheManager>();
            _baseSiteContextFactoryMock = new Mock<BaseSiteContextFactory>();
            var bytesConverterMock = new Mock<IBytesConverter>();
            _cleaners = new List<ICacheCleaner>();

            _cacheService = new CacheService(_baseSiteContextFactoryMock.Object, _baseCacheManagerMock.Object,
                bytesConverterMock.Object, _cleaners);
        }

        [Fact]
        public void ClearAll_ShouldBeOk()
        {
            // Arrange
            _baseCacheManagerMock.Setup(x => x.GetStatistics()).Returns(new CacheStatistics(1, 1));

            // Act
            var result = _cacheService.ClearAll();

            // Assert
            _baseCacheManagerMock.Verify(x => x.ClearAllCaches(), Times.Once);

            result.Should().NotBeNull();
            result.Successful.Should().BeTrue();
            result.OperationResults.Should().NotBeEmpty();
        }

        [Fact]
        public void ClearAll_CacheClearException_ShouldBeLogged()
        {
            // Arrange
            const string errorText = "FAKE_EXCEPTION";
            var exception = new Exception(errorText);
            _baseCacheManagerMock.Setup(x => x.GetStatistics()).Returns(new CacheStatistics(1, 1));
            _baseCacheManagerMock.Setup(x => x.ClearAllCaches()).Throws(exception);

            // Act
            var result = _cacheService.ClearAll();

            // Assert
            result.Should().NotBeNull();
            result.Successful.Should().BeFalse();
            result.OperationResults.Should().Contain(x => x.Messages.Any(m => m.Message.Contains(errorText)));
        }

        [Theory]
        [InlineData(CacheType.Data, CacheType.Html, CacheType.Item)]
        public void ClearBySite_AllCache_ShouldBeClearedAllCache(params CacheType[] cacheTypes)
        {
            // Arrange
            var cleaners = cacheTypes.Select(GetMockCacheCleaner).ToList();

            foreach (var cleaner in cleaners)
            {
                cleaner.Setup(x => x.Clear(It.IsAny<SiteContext>()))
                    .Returns(GetSuccessOperation(Enum.GetName(typeof(CacheType), cleaner.Object.CacheType)));
                _cleaners.Add(cleaner.Object);
            }

            _baseSiteContextFactoryMock.Setup(x => x.GetSiteContext(It.IsAny<string>()))
                .Returns(new SiteContext(SiteInfo.Create(new StringDictionary())));

            // Act
            var result = _cacheService.ClearBySite(It.IsAny<string>(), null);
            var operationResults = result.OperationResults.ToList();

            // Assert
            result.Should().NotBeNull();
            result.Successful.Should().BeTrue();
            operationResults.Should().NotBeEmpty();

            foreach (var cleaner in cleaners)
            {
                cleaner.Verify(x => x.Clear(It.IsAny<SiteContext>()), Times.Once);
            }
        }

        [Fact]
        public void ClearBySite_InvalidSite_ShouldThrow()
        {
            // Arrange
            _baseSiteContextFactoryMock.Setup(x => x.GetSiteContext(It.IsAny<string>()))
                .Returns((SiteContext)null);
            
            // Act
            Action action = () => { _cacheService.ClearBySite(It.IsAny<string>(), null); };

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(new[] { CacheType.Data, CacheType.Html, CacheType.Item },
            new[] { CacheType.Html, CacheType.Item, CacheType.Data })]
        [InlineData(new[] { CacheType.Data, CacheType.Html, CacheType.Item }, new[] { CacheType.Html, CacheType.Item })]
        [InlineData(new[] { CacheType.Data, CacheType.Html, CacheType.Item }, new[] { CacheType.Html })]
        public void ClearBySite_ClearSpecificSite_ShouldClearOnlySpecific(CacheType[] supportedCacheTypes,
            CacheType[] executedCacheTypes)
        {
            // Arrange
            var requestedCacheType = executedCacheTypes.Aggregate((CacheType)0, (res, type) => res | type);
            var supportedCleaners = supportedCacheTypes.Select(GetMockCacheCleaner).ToList();
            supportedCleaners.ForEach(c => c.Setup(x => x.Clear(It.IsAny<SiteContext>())).Returns(GetSuccessOperation("FakeCleaner")));
            var executedCleaners =
                supportedCleaners.Where(x => executedCacheTypes.Any(ect => ect == x.Object.CacheType))
                    .ToList();
            var ignoredCleaners = supportedCleaners.Except(executedCleaners).ToList();
            
            supportedCleaners.ForEach(c => _cleaners.Add(c.Object));
            
            _baseSiteContextFactoryMock.Setup(x => x.GetSiteContext(It.IsAny<string>()))
                .Returns(new SiteContext(SiteInfo.Create(new StringDictionary())));
            
            // Act
            var result = _cacheService.ClearBySite(It.IsAny<string>(), requestedCacheType);
            var operationResults = result.OperationResults.ToList();

            // Assert
            result.Should().NotBeNull();
            result.Successful.Should().BeTrue();
            operationResults.Should().NotBeEmpty();
            ignoredCleaners.ForEach(c => c.Verify(x => x.Clear(It.IsAny<SiteContext>()), Times.Never));
            executedCleaners.ForEach(c => c.Verify(x => x.Clear(It.IsAny<SiteContext>()), Times.Once));
        }

        private static OperationResult GetSuccessOperation(string name)
        {
            return new OperationResult
            {
                Name = name,
                Success = true,
                Messages = new List<OperationMessage>
                {
                    new OperationMessage(LogLevel.Information, CacheEventIds.AllCleared, "SUCCESS_MESSAGE")
                }
            };
        }

        private static Mock<ICacheCleaner> GetMockCacheCleaner(CacheType type)
        {
            var mockCleaner = new Mock<ICacheCleaner>();
            mockCleaner.SetupGet(x => x.CacheType).Returns(type);

            return mockCleaner;
        }
    }
}