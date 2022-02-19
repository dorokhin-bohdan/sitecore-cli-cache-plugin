using Microsoft.Extensions.Logging;
using Sitecore.Caching;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class RegistryCacheCleaner : BaseCacheCleaner
    {
        public override CacheType CacheType => CacheType.Registry;
        public override EventId EventId => CacheEventIds.RegistryCleared;

        public RegistryCacheCleaner(IBytesConverter bytesConverter) : base(bytesConverter)
        {
        }

        public override ICacheInfo GetCacheInfo(SiteContext context)
        {
            return context.Caches.RegistryCache.InnerCache;
        }
    }
}