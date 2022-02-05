using Sitecore.DevEx.Extensibility.Cache.Models;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services
{
    public interface ICacheService
    {
        void ClearBySite(string siteName, CacheType type);
        void ClearAll();
    }
}