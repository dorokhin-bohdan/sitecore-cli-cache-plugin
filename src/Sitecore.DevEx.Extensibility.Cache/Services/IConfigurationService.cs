using System.Threading.Tasks;
using Sitecore.DevEx.Configuration.Models;

namespace Sitecore.DevEx.Extensibility.Cache.Services
{
    public interface IConfigurationService
    {
        Task<RootConfiguration> GetRootConfigurationAsync(string configPath);
        Task<EnvironmentConfiguration> GetEnvironmentConfigurationAsync(string config, string envName);
    }
}