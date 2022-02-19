using Microsoft.Extensions.Logging;
using Sitecore.Caching;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class XslCacheCleaner : BaseCacheCleaner
    {
        public override CacheType CacheType => CacheType.Xsl;
        public override EventId EventId => CacheEventIds.XslCleared;

        public XslCacheCleaner(IBytesConverter bytesConverter) : base(bytesConverter)
        {
        }

        public override ICacheInfo GetCacheInfo(SiteContext context)
        {
            return context.Caches.XslCache.InnerCache;
        }
    }
}