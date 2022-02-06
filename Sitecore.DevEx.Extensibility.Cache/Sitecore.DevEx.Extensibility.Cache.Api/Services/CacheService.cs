using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Sitecore.Caching;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services
{
    public class CacheService : ICacheService
    {
        private readonly IEnumService _enumService;
        private readonly IBytesConverter _bytesConverter;

        public CacheService(IEnumService enumService, IBytesConverter bytesConverter)
        {
            _enumService = enumService;
            _bytesConverter = bytesConverter;
        }

        public CacheResultModel ClearBySite(string siteName, CacheType? type)
        {
            var result = new CacheResultModel();

            try
            {
                var site = SiteContext.GetSite(siteName);

                if (site == null)
                {
                    throw new ArgumentException($"The {siteName} site not found");
                }
                
                result.OperationResults = type == null
                    ? ClearAllCachesForSite(site)
                    : _enumService.GetFlagValues(type.Value)
                        .Select(cacheType => cacheType switch
                        {
                            CacheType.Data => ClearDataCache(site),
                            CacheType.Html => ClearHtmlCache(site),
                            CacheType.Item => ClearItemCache(site),
                            CacheType.Path => ClearPathCache(site),
                            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                        });
            }
            catch (Exception e)
            {
                result.Successful = false;
                result.OperationResults = new[] { OperationResult.FromException(e) };
            }

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
                    "[Cache] Cache cleared successfully."));
                cacheScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.AllCleared,
                    $"[Cache] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms."));
                result.OperationResults = new[] { cacheScope };
            }
            catch (Exception e)
            {
                result.Successful = false;
                result.OperationResults = new[] { OperationResult.FromException(e) };
            }

            return result;
        }

        private OperationResult ClearDataCache(SiteContext site)
        {
            var dataScope = new OperationResult("Data");
            var size = site.Database.Caches.DataCache.InnerCache.Size;

            var sw = Stopwatch.StartNew();
            site.Database.Caches.DataCache.Clear();
            sw.Stop();

            dataScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.DataCleared,
                "[Cache][Data] Data cache cleared successfully."));
            dataScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.DataCleared,
                $"[Cache][Data] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms."));

            return dataScope;
        }

        private OperationResult ClearHtmlCache(SiteContext site)
        {
            var htmlScope = new OperationResult("HTML");
            var size = site.Caches.HtmlCache.InnerCache.Size;

            var sw = Stopwatch.StartNew();
            site.Caches.HtmlCache.Clear();
            sw.Stop();

            htmlScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.HtmlCleared,
                "[Cache][HTML] Html cache cleared successfully."));
            htmlScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.HtmlCleared,
                $"[Cache][HTML] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms."));

            return htmlScope;
        }

        private OperationResult ClearItemCache(SiteContext site)
        {
            var itemScope = new OperationResult("Item");
            var size = site.Database.Caches.ItemCache.InnerCache.Size;

            var sw = Stopwatch.StartNew();
            site.Database.Caches.ItemCache.Clear();
            sw.Stop();

            itemScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.ItemCleared,
                "[Cache][Item] Item cache cleared successfully."));
            itemScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.ItemCleared,
                $"[Cache][Item] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms."));

            return itemScope;
        }

        private OperationResult ClearPathCache(SiteContext site)
        {
            var itemScope = new OperationResult("Item");
            var size = site.Database.Caches.PathCache.InnerCache.Size;

            var sw = Stopwatch.StartNew();
            site.Database.Caches.PathCache.Clear();
            sw.Stop();

            itemScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.PathCleared,
                "[Cache][Item] Item cache cleared successfully."));
            itemScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.PathCleared,
                $"[Cache][Item] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms."));

            return itemScope;
        }

        private IEnumerable<OperationResult> ClearAllCachesForSite(SiteContext site)
        {
            yield return ClearDataCache(site);
            yield return ClearHtmlCache(site);
            yield return ClearItemCache(site);
            yield return ClearPathCache(site);
        }
    }
}