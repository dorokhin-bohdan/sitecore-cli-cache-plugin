using System;
using System.Diagnostics;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class ViewStateCacheCleaner : ICacheCleaner
    {
        public CacheType CacheType => CacheType.RenderingParameters;

        private readonly IBytesConverter _bytesConverter;

        public ViewStateCacheCleaner(IBytesConverter bytesConverter)
        {
            _bytesConverter = bytesConverter;
        }

        public OperationResult Clear(SiteContext site)
        {
            var viewStateScope = new OperationResult("ViewState");
            var size = site.Caches.ViewStateCache.InnerCache.Size;

            try
            {
                var sw = Stopwatch.StartNew();
                site.Caches.ViewStateCache.Clear();
                sw.Stop();

                viewStateScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.ViewStateCleared,
                    "[Cache][ViewState] View State cache cleared successfully"));
                viewStateScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.ViewStateCleared,
                    $"[Cache][ViewState] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));
            }
            catch (Exception e)
            {
                viewStateScope.Chain(OperationResult.FromException(e));
                viewStateScope.Success = false;
            }

            return viewStateScope;
        }
    }
}