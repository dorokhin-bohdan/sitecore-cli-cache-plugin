using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sitecore.DependencyInjection;
using Sitecore.DevEx.Extensibility.Cache.Api.Controllers;
using Sitecore.DevEx.Extensibility.Cache.Api.Services;

namespace Sitecore.DevEx.Extensibility.Cache.Api
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            // Controllers
            serviceCollection.Replace(ServiceDescriptor.Transient(typeof(CacheCleanupController),
                typeof(CacheCleanupController)));
            
            // Services
            serviceCollection.AddTransient<IEnumService, EnumService>();
            serviceCollection.AddTransient<ICacheService, CacheService>();
            serviceCollection.AddTransient<IBytesConverter, BytesConverter>();
        }
    }
}