using System.Diagnostics;
using Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners
{
    public class DataCacheCleaner : ICacheCleaner
    {
        public CacheType CacheType => CacheType.Data;

        private readonly IBytesConverter _bytesConverter;

        public DataCacheCleaner(IBytesConverter bytesConverter)
        {
            _bytesConverter = bytesConverter;
        }

        public OperationResult Clear(SiteContext site)
        {
            var dataScope = new OperationResult("Data");
            var size = site.Database.Caches.DataCache.InnerCache.Size;

            var sw = Stopwatch.StartNew();
            site.Database.Caches.DataCache.Clear();
            sw.Stop();

            dataScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.DataCleared,
                "[Cache][Data] Data cache cleared successfully"));
            dataScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.DataCleared,
                $"[Cache][Data] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));

            return dataScope;
        }
    }
}