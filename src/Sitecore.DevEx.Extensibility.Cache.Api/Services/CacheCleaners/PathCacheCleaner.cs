using System.Diagnostics;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class PathCacheCleaner : ICacheCleaner
    {
        public CacheType CacheType => CacheType.Path;

        private readonly IBytesConverter _bytesConverter;

        public PathCacheCleaner(IBytesConverter bytesConverter)
        {
            _bytesConverter = bytesConverter;
        }

        public OperationResult Clear(SiteContext site)
        {
            var pathScope = new OperationResult("Path");
            var size = site.Database.Caches.PathCache.InnerCache.Size;

            var sw = Stopwatch.StartNew();
            site.Database.Caches.PathCache.Clear();
            sw.Stop();

            pathScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.PathCleared,
                "[Cache][Item] Path cache cleared successfully"));
            pathScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.PathCleared,
                $"[Cache][Item] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));

            return pathScope;
        }
    }
}