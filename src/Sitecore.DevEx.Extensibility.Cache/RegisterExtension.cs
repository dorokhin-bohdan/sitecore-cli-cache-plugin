using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sitecore.Devex.Client.Cli.Extensibility;
using Sitecore.Devex.Client.Cli.Extensibility.Subcommands;
using Sitecore.DevEx.Extensibility.Cache.Commands;
using Sitecore.DevEx.Extensibility.Cache.Constants;
using Sitecore.DevEx.Extensibility.Cache.Services;
using Sitecore.DevEx.Extensibility.Cache.Tasks;

namespace Sitecore.DevEx.Extensibility.Cache
{
    public class RegisterExtension : ISitecoreCliExtension
    {
        public void AddConfiguration(IConfigurationBuilder configBuilder)
        {
        }

        public void AddServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<CacheClearCommand>()
                .AddSingleton<SiteCacheClearCommand>()
                .AddSingleton<CacheClearTask>()
                .AddSingleton<SiteCacheClearTask>()
                .AddSingleton<IJsonService, JsonService>()
                .AddSingleton<ICacheApiClient, CacheApiClient>()
                .AddSingleton(sp => sp.GetService<ILoggerFactory>().CreateLogger<CacheClearTask>())
                .AddSingleton(sp => sp.GetService<ILoggerFactory>().CreateLogger<SiteCacheClearTask>())
                .AddSingleton(sp => sp.GetService<ILoggerFactory>().CreateLogger<CacheApiClient>());

            serviceCollection.AddHttpClient(CacheConstants.HttpClientName, client =>
                {
                    client.Timeout = TimeSpan.FromMinutes(5);
                })
                .ConfigurePrimaryHttpMessageHandler(_ => new CompressionAwareHttpClientHandler());
        }

        public IEnumerable<ISubcommand> AddCommands(IServiceProvider container)
        {
            return new[]
            {
                CreateCacheCommand(container)
            };
        }

        private static ISubcommand CreateCacheCommand(IServiceProvider container)
        {
            var cacheCommand = new CacheCommand("cache", "Cache commands for manage global cache and site cache.");
            cacheCommand.AddCommand(container.GetRequiredService<CacheClearCommand>());
            cacheCommand.AddCommand(container.GetRequiredService<SiteCacheClearCommand>());

            return cacheCommand;
        }
    }
}