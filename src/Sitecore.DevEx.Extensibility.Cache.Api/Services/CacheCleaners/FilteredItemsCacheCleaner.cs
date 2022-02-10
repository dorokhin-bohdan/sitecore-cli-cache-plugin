using System.Diagnostics;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class FilteredItemsCacheCleaner : ICacheCleaner
    {
        public CacheType CacheType => CacheType.FilteredItems;

        private readonly IBytesConverter _bytesConverter;

        public FilteredItemsCacheCleaner(IBytesConverter bytesConverter)
        {
            _bytesConverter = bytesConverter;
        }

        public OperationResult Clear(SiteContext site)
        {
            var filteredItemsScope = new OperationResult("FilteredItems");
            var size = site.Caches.FilteredItemsCache.InnerCache.Size;

            var sw = Stopwatch.StartNew();
            site.Caches.FilteredItemsCache.Clear();
            sw.Stop();

            filteredItemsScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.FilteredItemsCleared,
                "[Cache][FilteredItems] Filtered Items cache cleared successfully"));
            filteredItemsScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.FilteredItemsCleared,
                $"[Cache][FilteredItems] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));

            return filteredItemsScope;
        }
    }
}