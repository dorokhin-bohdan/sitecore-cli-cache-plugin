using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class FilteredItemsCacheCleaner : BaseCacheCleaner
    {
        public override CacheType CacheType => CacheType.FilteredItems;

        public FilteredItemsCacheCleaner(IBytesConverter bytesConverter) : base(bytesConverter)
        {
        }

        public override OperationResult Clear(SiteContext site)
        {
            return Clear(site.Caches.FilteredItemsCache.InnerCache);
        }
    }
}