using System.Diagnostics;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class HtmlCacheCleaner : ICacheCleaner
    {
        public CacheType CacheType => CacheType.Html;

        private readonly IBytesConverter _bytesConverter;

        public HtmlCacheCleaner(IBytesConverter bytesConverter)
        {
            _bytesConverter = bytesConverter;
        }

        public OperationResult Clear(SiteContext site)
        {
            var htmlScope = new OperationResult("HTML");
            var size = site.Caches.HtmlCache.InnerCache.Size;

            var sw = Stopwatch.StartNew();
            site.Caches.HtmlCache.Clear();
            sw.Stop();

            htmlScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.HtmlCleared,
                "[Cache][HTML] Html cache cleared successfully"));
            htmlScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.HtmlCleared,
                $"[Cache][HTML] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));

            return htmlScope;
        }
    }
}