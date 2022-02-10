using System;
using System.Diagnostics;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class RegistryCacheCleaner : ICacheCleaner
    {
        public CacheType CacheType => CacheType.Registry;

        private readonly IBytesConverter _bytesConverter;

        public RegistryCacheCleaner(IBytesConverter bytesConverter)
        {
            _bytesConverter = bytesConverter;
        }

        public OperationResult Clear(SiteContext site)
        {
            var registryScope = new OperationResult("Registry");
            var size = site.Caches.RegistryCache.InnerCache.Size;

            try
            {
                var sw = Stopwatch.StartNew();
                site.Caches.RegistryCache.Clear();
                sw.Stop();

                registryScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.RegistryCleared,
                    "[Cache][Registry] Registry values cache cleared successfully"));
                registryScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.RegistryCleared,
                    $"[Cache][Registry] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));
            }
            catch (Exception e)
            {
                registryScope.Chain(OperationResult.FromException(e));
                registryScope.Success = false;
            }

            return registryScope;
        }
    }
}