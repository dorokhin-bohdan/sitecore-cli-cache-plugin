using System;
using System.Diagnostics;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class ItemPathCacheCleaner : ICacheCleaner
    {
        public CacheType CacheType => CacheType.ItemPaths;

        private readonly IBytesConverter _bytesConverter;

        public ItemPathCacheCleaner(IBytesConverter bytesConverter)
        {
            _bytesConverter = bytesConverter;
        }

        public OperationResult Clear(SiteContext site)
        {
            var itemPathsScope = new OperationResult("ItemPaths");
            var size = site.Database.Caches.ItemPathsCache.InnerCache.Size;

            try
            {
                var sw = Stopwatch.StartNew();
                site.Database.Caches.ItemPathsCache.Clear();
                sw.Stop();

                itemPathsScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.ItemPathsCleared,
                    "[Cache][ItemPaths] Item paths cache cleared successfully"));
                itemPathsScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.ItemPathsCleared,
                    $"[Cache][ItemPaths] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));
            }
            catch (Exception e)
            {
                itemPathsScope.Chain(OperationResult.FromException(e));
                itemPathsScope.Success = false;
            }

            return itemPathsScope;
        }
    }
}