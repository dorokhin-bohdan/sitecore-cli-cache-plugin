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
            
            if (result.Successful)
            {
                Logger.LogConsoleInformation(string.Empty);
                Logger.LogConsoleInformation($"Clearing cache is finished", ConsoleColor.Green);
                Logger.LogConsoleVerbose($"Clearing cache is completed in {outerStopwatch.ElapsedMilliseconds}ms.", ConsoleColor.Yellow);   
            }
        }

        private static CacheType? GetCacheType(SiteCacheClearTaskOptions options)
        {
            CacheType? cacheType = null;

            if (options.ClearData)
                cacheType = (cacheType ?? 0) | CacheType.Data;

            if (options.ClearHtml)
                cacheType = (cacheType ?? 0) | CacheType.Html;
            
            if (options.ClearItem)
                cacheType = (cacheType ?? 0) | CacheType.Item;
            if (options.ClearPath)
                cacheType = (cacheType ?? 0) | CacheType.Path;
            
            if (options.ClearItemPaths)
                cacheType = (cacheType ?? 0) | CacheType.ItemPaths;
            
            if (options.ClearStandardValues)
                cacheType = (cacheType ?? 0) | CacheType.StandardValues;
            
            if (options.ClearFallback)
                cacheType = (cacheType ?? 0) | CacheType.IsFallbackValid;
            
            if (options.ClearRegistry)
                cacheType = (cacheType ?? 0) | CacheType.Registry;
            
            if (options.ClearXsl)
                cacheType = (cacheType ?? 0) | CacheType.Xsl;
            
            if (options.ClearFilteredItems)
                cacheType = (cacheType ?? 0) | CacheType.FilteredItems;
            
            if (options.ClearRenderingParams)
                cacheType = (cacheType ?? 0) | CacheType.RenderingParameters;
            
            if (options.ClearViewState)
                cacheType = (cacheType ?? 0) | CacheType.ViewState;

            return cacheType;
        }
    }
}