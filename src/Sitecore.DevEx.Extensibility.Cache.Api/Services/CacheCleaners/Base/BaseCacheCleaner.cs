using System;
using System.Diagnostics;
using Sitecore.Caching;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base
{
    public abstract class BaseCacheCleaner : ICacheCleaner
    {
        private readonly IBytesConverter _bytesConverter;

        public abstract CacheType CacheType { get; }
        private string Name => Enum.GetName(typeof(CacheType), CacheType);

        protected BaseCacheCleaner(IBytesConverter bytesConverter)
        {
            _bytesConverter = bytesConverter;
        }

        public abstract OperationResult Clear(SiteContext context);

        protected OperationResult Clear(ICacheInfo cacheInfo)
        {
            var scope = new OperationResult(Name);

            try
            {
                var size = cacheInfo.Size;

                var sw = Stopwatch.StartNew();
                cacheInfo.Clear();
                sw.Stop();

                scope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.DataCleared,
                    $"[Cache][{Name}] Cache cleared successfully"));
                scope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.DataCleared,
                    $"[Cache][{Name}] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));
            }
            catch (Exception e)
            {
                scope.Chain(OperationResult.FromException(e));
                scope.Success = false;
            }

            return scope;
        }
    }
}