using Sitecore.DevEx.Extensibility.Cache.Models;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services
{
    public interface ICacheService
    {
        CacheResultModel ClearBySite(string siteName, CacheType? type);
        CacheResultModel ClearAll();
    }
}