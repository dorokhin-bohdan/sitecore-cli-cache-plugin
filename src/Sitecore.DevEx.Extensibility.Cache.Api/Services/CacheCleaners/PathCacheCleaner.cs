﻿using System;
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

            try
            {
                var sw = Stopwatch.StartNew();
                site.Database.Caches.PathCache.Clear();
                sw.Stop();

                pathScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.PathCleared,
                    "[Cache][Path] Path cache cleared successfully"));
                pathScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.PathCleared,
                    $"[Cache][Path] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));
            }
            catch (Exception e)
            {
                pathScope.Chain(OperationResult.FromException(e));
                pathScope.Success = false;
            }

            return pathScope;
        }
    }
}