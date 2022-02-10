using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base
{
    public interface ICacheCleaner
    {
        CacheType CacheType { get; }

        OperationResult Clear(SiteContext context);
    }
}