using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sitecore.DevEx.Client.Logging;
using Sitecore.DevEx.Configuration;
using Sitecore.DevEx.Extensibility.Cache.Services;

namespace Sitecore.DevEx.Extensibility.Cache.Tasks
{
    public class CacheClearTask
    {
        private readonly IRootConfigurationManager _rootConfigurationManager;
        private readonly ILogger<CacheClearTask> _logger;
        private readonly ICacheApiClient _endpointService;

        public CacheClearTask(IRootConfigurationManager rootConfigurationManager, ILogger<CacheClearTask> logger, ICacheApiClient endpointService)
        {
            _rootConfigurationManager = rootConfigurationManager;
            _logger = logger;
            _endpointService = endpointService;
        }

        public async Task Execute(CacheClearTaskOptions options)
        {
            _logger.LogConsoleVerbose("Welcome to Cache plugin!");

            options.Validate();

            var outerStopwatch = Stopwatch.StartNew();

            var rootConfiguration = await _rootConfigurationManager.ResolveRootConfiguration(options.Config);

            if (!rootConfiguration.Environments.TryGetValue(options.EnvironmentName, out var environmentConfiguration))
            {
                throw new InvalidConfigurationException($"Environment {options.EnvironmentName} was not defined. Use the login command to define it.");
            }
            
            var result = await _endpointService.ClearAllAsync(environmentConfiguration);

            outerStopwatch.Stop();

            _logger.LogConsoleVerbose(string.Empty);
            _logger.LogConsoleVerbose($"Clearing cache is finished in {outerStopwatch.ElapsedMilliseconds}ms.");
        }
    }
}