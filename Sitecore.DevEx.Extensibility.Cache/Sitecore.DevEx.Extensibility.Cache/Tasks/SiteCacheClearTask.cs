using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sitecore.DevEx.Client.Logging;
using Sitecore.DevEx.Configuration;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Extensibility.Cache.Models.Requests;
using Sitecore.DevEx.Extensibility.Cache.Services;

namespace Sitecore.DevEx.Extensibility.Cache.Tasks
{
    public class SiteCacheClearTask
    {
        private readonly IRootConfigurationManager _rootConfigurationManager;
        private readonly ILogger<SiteCacheClearTask> _logger;
        private readonly ICacheApiClient _endpointService;

        public SiteCacheClearTask(IRootConfigurationManager rootConfigurationManager, ILogger<SiteCacheClearTask> logger, ICacheApiClient endpointService)
        {
            _rootConfigurationManager = rootConfigurationManager;
            _logger = logger;
            _endpointService = endpointService;
        }

        public async Task Execute(SiteCacheClearTaskOptions options)
        {
            options.Validate();

            var outerStopwatch = Stopwatch.StartNew();

            var rootConfiguration = await _rootConfigurationManager.ResolveRootConfiguration(options.Config);

            if (!rootConfiguration.Environments.TryGetValue(options.EnvironmentName, out var environmentConfiguration))
            {
                throw new InvalidConfigurationException($"Environment {options.EnvironmentName} was not defined. Use the login command to define it.");
            }

            CacheType? cacheType = null;

            if (options.Data)
                cacheType = cacheType.HasValue 
                    ? cacheType | CacheType.Data
                    : CacheType.Data;
            
            if (options.Html)
                cacheType = cacheType.HasValue 
                    ? cacheType | CacheType.Html
                    : CacheType.Html;
            
            if (options.Item)
                cacheType = cacheType.HasValue 
                    ? cacheType | CacheType.Item
                    : CacheType.Item;
            
            if (options.Path)
                cacheType = cacheType.HasValue 
                    ? cacheType | CacheType.Path
                    : CacheType.Path;
            
            var request = new CacheCleanupRequest
            {
                Site = options.SiteName,
                CacheType = cacheType ?? CacheType.All
            };
            
            var result = await _endpointService.ClearBySiteAsync(environmentConfiguration, request).ConfigureAwait(false);

            outerStopwatch.Stop();

            _logger.LogConsoleVerbose(string.Empty);
            _logger.LogConsoleVerbose($"Clearing cache is finished in {outerStopwatch.ElapsedMilliseconds}ms.");
        }
    }
}