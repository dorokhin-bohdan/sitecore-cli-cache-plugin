using Sitecore.Caching;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class RenderingParametersCacheCleaner : BaseCacheCleaner
    {
        public override CacheType CacheType => CacheType.RenderingParameters;

        public RenderingParametersCacheCleaner(IBytesConverter bytesConverter) : base(bytesConverter)
        {
        }

        public override ICacheInfo GetCacheInfo(SiteContext context)
        {
            return context.Caches.RenderingParametersCache.InnerCache;
        }
    }
}