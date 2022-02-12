using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class DataCacheCleaner : BaseCacheCleaner
    {
        public override CacheType CacheType => CacheType.Data;

        public DataCacheCleaner(IBytesConverter bytesConverter) : base(bytesConverter)
        {
        }
        
        public override OperationResult Clear(SiteContext site)
        {
            return Clear(site.Database.Caches.DataCache.InnerCache);
        }
    }
}