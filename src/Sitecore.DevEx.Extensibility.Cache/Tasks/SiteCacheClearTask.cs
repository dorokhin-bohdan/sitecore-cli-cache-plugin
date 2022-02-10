using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sitecore.DevEx.Client.Logging;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Extensibility.Cache.Models.Requests;
using Sitecore.DevEx.Extensibility.Cache.Services;
using Sitecore.DevEx.Extensibility.Cache.Tasks.Base;

namespace Sitecore.DevEx.Extensibility.Cache.Tasks
{
    public class SiteCacheClearTask : BaseCacheClearTask
    {
        private readonly ICacheApiClient _endpointService;

        public SiteCacheClearTask(
            IConfigurationService configurationService,
            ILogger<SiteCacheClearTask> logger, 
            ICacheApiClient endpointService) : base(configurationService, logger)
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
                await ConfigurationService.GetEnvironmentConfigurationAsync(options.Config, options.EnvironmentName);
            var result = await _endpointService.ClearBySiteAsync(environmentConfiguration, request).ConfigureAwait(false);
            outerStopwatch.Stop();
            
            if (result == null)
                return;
            
            PrintLogs(result.OperationResults);
            
            if (result.Successful)
            {
                Logger.LogConsoleInformation($"Clearing cache for \"{options.SiteName}\" is finished", ConsoleColor.Green);
                Logger.LogConsoleVerbose($"Operation completed in {outerStopwatch.ElapsedMilliseconds}ms.", ConsoleColor.Yellow);   
            }
        }

        private static CacheType? GetCacheType(SiteCacheClearTaskOptions options)
        {
            CacheType? cacheType = null;

            if (options.ClearData)
                AddCacheType(CacheType.Data, ref cacheType);

            if (options.ClearHtml)
                AddCacheType(CacheType.Html, ref cacheType);
            
            if (options.ClearItem)
                AddCacheType(CacheType.Item, ref cacheType);
            
            if (options.ClearPath)
                AddCacheType(CacheType.Path, ref cacheType);
            
            if (options.ClearItemPaths)
                AddCacheType(CacheType.ItemPaths, ref cacheType);
            
            if (options.ClearStandardValues)
                AddCacheType(CacheType.StandardValues, ref cacheType);
            
            if (options.ClearFallback)
                AddCacheType(CacheType.IsFallbackValid, ref cacheType);
            
            if (options.ClearRegistry)
                AddCacheType(CacheType.Registry, ref cacheType);
            
            if (options.ClearXsl)
                AddCacheType(CacheType.Xsl, ref cacheType);
            
            if (options.ClearFilteredItems)
                AddCacheType(CacheType.FilteredItems, ref cacheType);
            
            if (options.ClearRenderingParams)
                AddCacheType(CacheType.RenderingParameters, ref cacheType);
            
            if (options.ClearViewState)
                AddCacheType(CacheType.ViewState, ref cacheType);

            return cacheType;

            void AddCacheType(CacheType newCacheType, ref CacheType? result)
            {
                result = (result ?? 0) | newCacheType;
            }
        }
    }
}