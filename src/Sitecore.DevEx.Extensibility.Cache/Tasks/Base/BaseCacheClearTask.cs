using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sitecore.DevEx.Client.Logging;
using Sitecore.DevEx.Configuration;
using Sitecore.DevEx.Configuration.Models;
using Sitecore.DevEx.Extensibility.Cache.Services;
using Sitecore.DevEx.Logging;

namespace Sitecore.DevEx.Extensibility.Cache.Tasks.Base
{
    public abstract class BaseCacheClearTask
    {
        protected readonly IConfigurationService ConfigurationService;
        protected readonly ILogger<BaseCacheClearTask> Logger;

        protected BaseCacheClearTask(IConfigurationService configurationService, ILogger<BaseCacheClearTask> logger)
        {
            ConfigurationService = configurationService;
            Logger = logger;
        }

        protected virtual void PrintLogs(IEnumerable<OperationResult> operationResults)
        {
            foreach (var operationResult in operationResults)
            {
                foreach (var message in operationResult.Messages)
                {
                    switch (message.LogLevel)
                    {
                        case LogLevel.Debug:
                            Logger.LogConsoleVerbose(message.Message, ConsoleColor.Yellow);
                            break;
                        case LogLevel.Information:
                            Logger.LogConsoleInformation(message.Message, ConsoleColor.Green);
                            break;
                        case LogLevel.Trace:
                        case LogLevel.Warning:
                        case LogLevel.Error:
                        case LogLevel.Critical:
                        case LogLevel.None:
                            Logger.LogConsole(message.LogLevel, message.Message);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }
}