﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sitecore.DevEx.Client.Logging;
using Sitecore.DevEx.Extensibility.Cache.Services;
using Sitecore.DevEx.Extensibility.Cache.Tasks.Base;

namespace Sitecore.DevEx.Extensibility.Cache.Tasks;

public class CacheClearTask : BaseCacheClearTask
{
    private readonly ICacheApiClient _cacheApiClient;

    public CacheClearTask(
        IConfigurationService configurationService, 
        ILogger<CacheClearTask> logger,
        ICacheApiClient cacheApiClient) : base(configurationService, logger)
    {
        _cacheApiClient = cacheApiClient;
    }

    public async Task Execute(CacheClearTaskOptions options)
    {
        options.Validate();
        Logger.LogConsoleInformation("Starting clearing cache for Sitecore.", ConsoleColor.DarkGreen);
            
        var outerStopwatch = Stopwatch.StartNew();
        var environmentConfiguration = await ConfigurationService.GetEnvironmentConfigurationAsync(options.Config, options.EnvironmentName);
        var result = await _cacheApiClient.ClearAllAsync(environmentConfiguration).ConfigureAwait(false);
        outerStopwatch.Stop();

        if (result == null)
            return;
            
        PrintLogs(result.OperationResults);
            
        if (result.Successful)
        {
            Logger.LogConsoleInformation($"Clearing cache is finished.", ConsoleColor.Green);
            Logger.LogConsoleVerbose($"Operation completed in {outerStopwatch.ElapsedMilliseconds}ms.", ConsoleColor.Yellow);
        }
    }
}