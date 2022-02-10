using System.Diagnostics;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class StandardValuesCacheCleaner : ICacheCleaner
    {
        public CacheType CacheType => CacheType.StandardValues;

        private readonly IBytesConverter _bytesConverter;

        public StandardValuesCacheCleaner(IBytesConverter bytesConverter)
        {
            _bytesConverter = bytesConverter;
        }

        public OperationResult Clear(SiteContext site)
        {
            var svScope = new OperationResult("StandardValues");
            var size = site.Database.Caches.StandardValuesCache.InnerCache.Size;

            var sw = Stopwatch.StartNew();
            site.Database.Caches.StandardValuesCache.Clear();
            sw.Stop();

            svScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.StandardValuesCleared,
                "[Cache][StandardValues] Standard values cache cleared successfully"));
            svScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.StandardValuesCleared,
                $"[Cache][StandardValues] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));

            return svScope;
        }
    }
}