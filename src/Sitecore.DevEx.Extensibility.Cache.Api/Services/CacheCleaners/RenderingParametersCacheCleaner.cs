using System;
using System.Diagnostics;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class RenderingParametersCacheCleaner : ICacheCleaner
    {
        public CacheType CacheType => CacheType.RenderingParameters;

        private readonly IBytesConverter _bytesConverter;

        public RenderingParametersCacheCleaner(IBytesConverter bytesConverter)
        {
            _bytesConverter = bytesConverter;
        }

        public OperationResult Clear(SiteContext site)
        {
            var renderingParamsScope = new OperationResult("RenderingParameters");
            var size = site.Caches.RenderingParametersCache.InnerCache.Size;

            try
            {
                var sw = Stopwatch.StartNew();
                site.Caches.RenderingParametersCache.Clear();
                sw.Stop();

                renderingParamsScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.RenderingParametersCleared,
                    "[Cache][RenderingParameters] Rendering Parameters cache cleared successfully"));
                renderingParamsScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.RenderingParametersCleared,
                    $"[Cache][RenderingParameters] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));
            }
            catch (Exception e)
            {
                renderingParamsScope.Chain(OperationResult.FromException(e));
                renderingParamsScope.Success = false;
            }

            return renderingParamsScope;
        }
    }
}