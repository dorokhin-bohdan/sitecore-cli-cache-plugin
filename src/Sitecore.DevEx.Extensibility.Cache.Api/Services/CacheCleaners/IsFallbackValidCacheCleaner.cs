using Sitecore.Caching;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class IsFallbackValidCacheCleaner : BaseCacheCleaner
    {
        public override CacheType CacheType => CacheType.IsFallbackValid;
        
        public IsFallbackValidCacheCleaner(IBytesConverter bytesConverter) : base(bytesConverter)
        {
        }

        public override ICacheInfo GetCacheInfo(SiteContext context)
        {
            return context.Database.Caches.IsFallbackValidCache.InnerCache;
        }
    }
}