using System.Diagnostics;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class ItemCacheCleaner : ICacheCleaner
    {
        public CacheType CacheType => CacheType.Item;

        private readonly IBytesConverter _bytesConverter;

        public ItemCacheCleaner(IBytesConverter bytesConverter)
        {
            _bytesConverter = bytesConverter;
        }

        public OperationResult Clear(SiteContext site)
        {
            var itemScope = new OperationResult("Item");
            var size = site.Database.Caches.ItemCache.InnerCache.Size;

            var sw = Stopwatch.StartNew();
            site.Database.Caches.ItemCache.Clear();
            sw.Stop();

            itemScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.ItemCleared,
                "[Cache][Item] Item cache cleared successfully"));
            itemScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.ItemCleared,
                $"[Cache][Item] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));

            return itemScope;
        }
    }
}