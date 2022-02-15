using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Sitecore.Abstractions;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services
{
    public class CacheService : ICacheService
    {
        private readonly BaseSiteContextFactory _siteContextFactory;
        private readonly BaseCacheManager _cacheManager;
        private readonly IBytesConverter _bytesConverter;
        private readonly IEnumerable<ICacheCleaner> _cacheCleaners;

        public CacheService(
            BaseSiteContextFactory siteContextFactory,
            BaseCacheManager cacheManager,
            IBytesConverter bytesConverter,
            IEnumerable<ICacheCleaner> cacheCleaners)
        {
            _siteContextFactory = siteContextFactory;
            _cacheManager = cacheManager;
            _bytesConverter = bytesConverter;
            _cacheCleaners = cacheCleaners;
        }

        public CacheResultModel ClearBySite(string siteName, CacheType? type)
        {
            var site = _siteContextFactory.GetSiteContext(siteName);

            if (site == null)
            {
                throw new ArgumentException($"The {siteName} site not found");
            }

            var result = new CacheResultModel
            {
                OperationResults = ClearCachesForSite(site, type)
            };

            return result;
        }

        public CacheResultModel ClearAll()
        {
            var result = new CacheResultModel();

            try
            {
                var cacheScope = new OperationResult("Cache");
                var size = _cacheManager.GetStatistics().TotalSize;

                var sw = Stopwatch.StartNew();
                _cacheManager.ClearAllCaches();
                sw.Stop();

                cacheScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.AllCleared,
                    "[Cache] Cache cleared successfully"));
                cacheScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.AllCleared,
                    $"[Cache] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));
                result.OperationResults = new[] { cacheScope };
            }
            catch (Exception e)
            {
                result.Successful = false;
                result.OperationResults = new[] { OperationResult.FromException(e) };
            }

            return result;
        }


        private IEnumerable<OperationResult> ClearCachesForSite(SiteContext site, CacheType? type)
        {
            var involvedCleaners = _cacheCleaners;

            if (type != null)
                involvedCleaners = _cacheCleaners.Where(cc => (type & cc.CacheType) != 0);

            foreach (var cacheCleaner in involvedCleaners)
                yield return cacheCleaner.Clear(site);
        }
    }
}