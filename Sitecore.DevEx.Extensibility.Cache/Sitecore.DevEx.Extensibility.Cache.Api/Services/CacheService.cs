using System;
using Sitecore.Caching;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services
{
    public class CacheService : ICacheService
    {
        private readonly IEnumService _enumService;
        
        public CacheService(IEnumService enumService)
        {
            _enumService = enumService;
        }
        
        public void ClearBySite(string siteName, CacheType type)
        {
            var site = SiteContext.GetSite(siteName);
            var types = _enumService.GetFlagValues(type);
            
            foreach (var cacheType in types)
            {
                switch (cacheType)
                {
                    case CacheType.All:
                        site.Caches.HtmlCache.Clear();
                        site.Database.Caches.DataCache.Clear(); 
                        site.Database.Caches.DataCache.Clear();
                        site.Database.Caches.PathCache.Clear();
                        break;
                    case CacheType.Data:
                        site.Database.Caches.DataCache.Clear(); 
                        break;
                    case CacheType.Html:
                        site.Caches.HtmlCache.Clear();
                        break;
                    case CacheType.Item:
                        site.Database.Caches.DataCache.Clear();
                        break;
                    case CacheType.Path:
                        site.Database.Caches.PathCache.Clear();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }
        }

        public void ClearAll()
        {
            CacheManager.ClearAllCaches();
        }
    }
}