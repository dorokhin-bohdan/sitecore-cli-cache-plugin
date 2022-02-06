using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sitecore.DevEx.Client.Logging;
using Sitecore.DevEx.Configuration;
using Sitecore.DevEx.Configuration.Models;
using Sitecore.DevEx.Logging;

namespace Sitecore.DevEx.Extensibility.Cache.Tasks
{
    public abstract class BaseCacheClearTask
    {
        private readonly IRootConfigurationManager _rootConfigurationManager;
        protected readonly ILogger<BaseCacheClearTask> Logger;

        protected BaseCacheClearTask(IRootConfigurationManager rootConfigurationManager, ILogger<BaseCacheClearTask> logger)
        {
            _rootConfigurationManager = rootConfigurationManager;
            Logger = logger;
        }

        protected virtual async Task<EnvironmentConfiguration> GetEnvironmentConfigurationAsync(string config, string envName)
        {
            var rootConfiguration = await _rootConfigurationManager.ResolveRootConfiguration(config);

            if (!rootConfiguration.Environments.TryGetValue(envName, out var environmentConfiguration))
            {
                throw new InvalidConfigurationException(
                    $"Environment {envName} was not defined. Use the login command to define it.");
            }

            return environmentConfiguration;
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