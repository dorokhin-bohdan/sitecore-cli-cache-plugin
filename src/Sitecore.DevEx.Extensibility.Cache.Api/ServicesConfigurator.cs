using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sitecore.DependencyInjection;
using Sitecore.DevEx.Extensibility.Cache.Api.Controllers;
using Sitecore.DevEx.Extensibility.Cache.Api.Services;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;

namespace Sitecore.DevEx.Extensibility.Cache.Api
{
    [ExcludeFromCodeCoverage]
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            // Controllers
            serviceCollection.Replace(ServiceDescriptor.Transient(typeof(CacheCleanupController),
                typeof(CacheCleanupController)));
            
            // Services
            serviceCollection.AddTransient<ICacheService, CacheService>();
            serviceCollection.AddTransient<IBytesConverter, BytesConverter>();
            
            ConfigureCacheCleaners(serviceCollection);
        }

        private static void ConfigureCacheCleaners(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICacheCleaner, DataCacheCleaner>();
            serviceCollection.AddTransient<ICacheCleaner, FilteredItemsCacheCleaner>();
            serviceCollection.AddTransient<ICacheCleaner, HtmlCacheCleaner>();
            serviceCollection.AddTransient<ICacheCleaner, IsFallbackValidCacheCleaner>();
            serviceCollection.AddTransient<ICacheCleaner, ItemCacheCleaner>();
            serviceCollection.AddTransient<ICacheCleaner, ItemPathCacheCleaner>();
            serviceCollection.AddTransient<ICacheCleaner, PathCacheCleaner>();
            serviceCollection.AddTransient<ICacheCleaner, RegistryCacheCleaner>();
            serviceCollection.AddTransient<ICacheCleaner, RenderingParametersCacheCleaner>();
            serviceCollection.AddTransient<ICacheCleaner, StandardValuesCacheCleaner>();
            serviceCollection.AddTransient<ICacheCleaner, ViewStateCacheCleaner>();
            serviceCollection.AddTransient<ICacheCleaner, XslCacheCleaner>();
        }
    }
}