using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Sitecore.Caching;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services.CacheCleaners.Base
{
    public abstract class BaseCacheCleaner : ICacheCleaner
    {
        public abstract CacheType CacheType { get; }
        public abstract EventId EventId { get; }

        private readonly IBytesConverter _bytesConverter;
        private string Name => Enum.GetName(typeof(CacheType), CacheType);

        protected BaseCacheCleaner(IBytesConverter bytesConverter)
        {
            _bytesConverter = bytesConverter;
        }

        public abstract ICacheInfo GetCacheInfo(SiteContext context);

        public OperationResult Clear(SiteContext context)
        {
            return Clear(GetCacheInfo(context));
        }

        private OperationResult Clear(ICacheInfo cacheInfo)
        {
            var scope = new OperationResult(Name);

            try
            {
                var size = cacheInfo.Size;

                var sw = Stopwatch.StartNew();
                cacheInfo.Clear();
                sw.Stop();

                scope.Chain(OperationResult.FromInfoSuccess(EventId,
                    $"[Cache][{Name}] Cache cleared successfully."));
                scope.Chain(OperationResult.FromVerboseSuccess(EventId,
                    $"[Cache][{Name}] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms."));
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