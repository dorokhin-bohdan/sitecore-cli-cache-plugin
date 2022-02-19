using Microsoft.Extensions.Logging;
using Sitecore.Caching;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class ViewStateCacheCleaner : BaseCacheCleaner
    {
        public override CacheType CacheType => CacheType.ViewState;
        public override EventId EventId => CacheEventIds.ViewStateCleared;
        
        public ViewStateCacheCleaner(IBytesConverter bytesConverter) : base(bytesConverter)
        {
        }

        public override ICacheInfo GetCacheInfo(SiteContext context)
        {
            return context.Caches.ViewStateCache.InnerCache;
        }
    }
}