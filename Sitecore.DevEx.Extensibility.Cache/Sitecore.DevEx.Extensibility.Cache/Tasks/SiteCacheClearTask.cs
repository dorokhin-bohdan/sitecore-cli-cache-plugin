using System;
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
    public class SiteCacheClearTask : BaseCacheClearTask
    {
        private readonly ICacheApiClient _endpointService;

        public SiteCacheClearTask(IRootConfigurationManager rootConfigurationManager,
            ILogger<SiteCacheClearTask> logger, ICacheApiClient endpointService)
            : base(rootConfigurationManager, logger)
        {
            _endpointService = endpointService;
        }

        public async Task Execute(SiteCacheClearTaskOptions options)
        {
            options.Validate();
            Logger.LogConsoleInformation($"Starting clearing cache for \"{options.SiteName}\" site", ConsoleColor.DarkGreen);

            var outerStopwatch = Stopwatch.StartNew();
            var request = new CacheCleanupRequest
            {
                Site = options.SiteName,
                CacheType = GetCacheType(options)
            };

            var environmentConfiguration =
                await GetEnvironmentConfigurationAsync(options.Config, options.EnvironmentName);
            var result = await _endpointService.ClearBySiteAsync(environmentConfiguration, request).ConfigureAwait(false);
            outerStopwatch.Stop();
            
            PrintLogs(result.OperationResults);
            Logger.LogConsoleInformation(string.Empty);
            Logger.LogConsoleInformation($"Clearing cache is finished", ConsoleColor.Green);
            Logger.LogConsoleVerbose($"Clearing cache is completed in {outerStopwatch.ElapsedMilliseconds}ms.", ConsoleColor.Yellow);
        }

        private CacheType? GetCacheType(SiteCacheClearTaskOptions options)
        {
            CacheType? cacheType = null;

            if (options.CleanData)
                cacheType = cacheType.HasValue
                    ? cacheType | CacheType.Data
                    : CacheType.Data;

            if (options.CleanHtml)
                cacheType = cacheType.HasValue
                    ? cacheType | CacheType.Html
                    : CacheType.Html;

            if (options.CleanItem)
                cacheType = cacheType.HasValue
                    ? cacheType | CacheType.Item
                    : CacheType.Item;

            if (options.CleanPath)
                cacheType = cacheType.HasValue
                    ? cacheType | CacheType.Path
                    : CacheType.Path;

            return cacheType;
        }
    }
}