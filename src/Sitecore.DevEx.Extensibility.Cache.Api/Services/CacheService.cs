using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Sitecore.Caching;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services
{
    public class CacheService : ICacheService
    {
        private readonly IEnumService _enumService;
        private readonly IBytesConverter _bytesConverter;
        private readonly IReadOnlyCollection<ICacheCleaner> _cacheCleaners;

        public CacheService(IEnumService enumService, IBytesConverter bytesConverter,
            IReadOnlyCollection<ICacheCleaner> cacheCleaners)
        {
            _enumService = enumService;
            _bytesConverter = bytesConverter;
            _cacheCleaners = cacheCleaners;
        }

        public CacheResultModel ClearBySite(string siteName, CacheType? type)
        {
            var site = SiteContext.GetSite(siteName);

            if (site == null)
            {
                throw new ArgumentException($"The {siteName} site not found");
            }

            var result = new CacheResultModel
            {
                OperationResults = type == null
                    ? ClearAllCachesForSite(site)
                    : _enumService.GetFlagValues(type.Value)
                        .Select(cacheType =>
                            _cacheCleaners.FirstOrDefault(cc => cc.CacheType == cacheType)?.Clear(site))
            };

            return result;
        }

        public CacheResultModel ClearAll()
        {
            var result = new CacheResultModel();

            try
            {
                var cacheScope = new OperationResult("Cache");
                var size = CacheManager.GetStatistics().TotalSize;

                var sw = Stopwatch.StartNew();
                CacheManager.ClearAllCaches();
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


        private IEnumerable<OperationResult> ClearAllCachesForSite(SiteContext site)
        {
            foreach (var cacheCleaner in _cacheCleaners)
                yield return cacheCleaner.Clear(site);
        }
    }
}