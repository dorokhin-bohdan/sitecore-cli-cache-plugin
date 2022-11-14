using System.Threading.Tasks;
using Sitecore.DevEx.Configuration.Models;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Extensibility.Cache.Models.Requests;

namespace Sitecore.DevEx.Extensibility.Cache.Services;

public interface ICacheApiClient
{
    Task<CacheResultModel> ClearAllAsync(EnvironmentConfiguration configuration);
        
    Task<CacheResultModel> ClearBySiteAsync(EnvironmentConfiguration configuration, CacheCleanupRequest request);
}