using System.Threading.Tasks;
using Sitecore.DevEx.Configuration;
using Sitecore.DevEx.Configuration.Models;

namespace Sitecore.DevEx.Extensibility.Cache.Services;

public class ConfigurationService : IConfigurationService
{
    private readonly IRootConfigurationManager _rootConfigurationManager;

    public ConfigurationService(IRootConfigurationManager rootConfigurationManager)
    {
        _rootConfigurationManager = rootConfigurationManager;
    }

    public Task<RootConfiguration> GetRootConfigurationAsync(string configPath)
    {
        return _rootConfigurationManager.ResolveRootConfiguration(configPath);
    }
        
    public async Task<EnvironmentConfiguration> GetEnvironmentConfigurationAsync(string config, string envName)
    {
        var rootConfiguration = await GetRootConfigurationAsync(config);

        if (!rootConfiguration.Environments.TryGetValue(envName, out var environmentConfiguration))
        {
            throw new InvalidConfigurationException(
                $"Environment {envName} was not defined. Use the login command to define it.");
        }

        return environmentConfiguration;
    }
}