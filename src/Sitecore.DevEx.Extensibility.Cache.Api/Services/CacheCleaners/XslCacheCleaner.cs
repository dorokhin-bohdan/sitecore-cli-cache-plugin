using System;
using System.Diagnostics;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class XslCacheCleaner : ICacheCleaner
    {
        public CacheType CacheType => CacheType.Xsl;

        private readonly IBytesConverter _bytesConverter;

        public XslCacheCleaner(IBytesConverter bytesConverter)
        {
            _bytesConverter = bytesConverter;
        }

        public OperationResult Clear(SiteContext site)
        {
            var xslScope = new OperationResult("Xsl");
            var size = site.Caches.XslCache.InnerCache.Size;

            try
            {
                var sw = Stopwatch.StartNew();
                site.Caches.XslCache.Clear();
                sw.Stop();

                xslScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.XslCleared,
                    "[Cache][XSL] XSL cache cleared successfully"));
                xslScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.XslCleared,
                    $"[Cache][XSL] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));
            }
            catch (Exception e)
            {
                xslScope.Chain(OperationResult.FromException(e));
                xslScope.Success = false;
            }

            return xslScope;
        }
    }
}