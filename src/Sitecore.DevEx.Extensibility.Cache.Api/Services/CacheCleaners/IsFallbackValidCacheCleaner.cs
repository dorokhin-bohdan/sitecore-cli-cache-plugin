using System;
using System.Diagnostics;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class IsFallbackValidCacheCleaner : ICacheCleaner
    {
        public CacheType CacheType => CacheType.IsFallbackValid;

        private readonly IBytesConverter _bytesConverter;

        public IsFallbackValidCacheCleaner(IBytesConverter bytesConverter)
        {
            _bytesConverter = bytesConverter;
        }

        public OperationResult Clear(SiteContext site)
        {
            var fallbackScope = new OperationResult("IsFallbackValid");
            var size = site.Database.Caches.IsFallbackValidCache.InnerCache.Size;

            try
            {
                var sw = Stopwatch.StartNew();
                site.Database.Caches.IsFallbackValidCache.Clear();
                sw.Stop();

                fallbackScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.IsFallbackValidCleared,
                    "[Cache][IsFallbackValid] Is Fallback Valid values cache cleared successfully"));
                fallbackScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.IsFallbackValidCleared,
                    $"[Cache][IsFallbackValid] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));
            }
            catch (Exception e)
            {
                fallbackScope.Chain(OperationResult.FromException(e));
                fallbackScope.Success = false;
            }
            
            return fallbackScope;
        }
    }
}