using Microsoft.Extensions.Logging;
using Sitecore.Caching;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class ItemPathCacheCleaner : BaseCacheCleaner
    {
        public override CacheType CacheType => CacheType.ItemPaths;
        public override EventId EventId => CacheEventIds.ItemPathsCleared;

        public ItemPathCacheCleaner(IBytesConverter bytesConverter) : base(bytesConverter)
        {
        }

        public override ICacheInfo GetCacheInfo(SiteContext context)
        {
            return context.Database.Caches.ItemPathsCache.InnerCache;
        }
    }
}