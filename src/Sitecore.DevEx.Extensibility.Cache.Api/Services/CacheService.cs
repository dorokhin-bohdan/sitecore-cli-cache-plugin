using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Sitecore.Caching;
using Sitecore.Caching.Generics;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Logging;
using Sitecore.Sites;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Services
{
    public class CacheService : ICacheService
    {
        private readonly IEnumService _enumService;
        private readonly IBytesConverter _bytesConverter;

        public CacheService(IEnumService enumService, IBytesConverter bytesConverter)
        {
            _enumService = enumService;
            _bytesConverter = bytesConverter;
        }

        public CacheResultModel ClearBySite(string siteName, CacheType? type)
        {
            var result = new CacheResultModel();

            try
            {
                var site = SiteContext.GetSite(siteName);

                if (site == null)
                {
                    throw new ArgumentException($"The {siteName} site not found");
                }

                result.OperationResults = type == null
                    ? ClearAllCachesForSite(site)
                    : _enumService.GetFlagValues(type.Value)
                        .Select(cacheType => cacheType switch
                        {
                            CacheType.Data => ClearDataCache(site),
                            CacheType.Html => ClearHtmlCache(site),
                            CacheType.Item => ClearItemCache(site),
                            CacheType.Path => ClearPathCache(site),
                            CacheType.ItemPaths => ClearItemCache(site),
                            CacheType.StandardValues => ClearStandardValuesCache(site),
                            CacheType.IsFallbackValid => ClearIsFallbackValidCache(site),
                            CacheType.Registry => ClearRegistryCache(site),
                            CacheType.Xsl => ClearXslCache(site),
                            CacheType.FilteredItems => ClearFilteredItemsCache(site),
                            CacheType.RenderingParameters => ClearRenderingParametersCache(site),
                            CacheType.ViewState => ClearViewStateCache(site),
                            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                        });
            }
            catch (Exception e)
            {
                result.Successful = false;
                result.OperationResults = new[] { OperationResult.FromException(e) };
            }

            return result;
        }

        public CacheResultModel ClearAll()
        {
            var result = new CacheResultModel();

            try
            {
                var cacheScope = new OperationResult("Cache");
                var size = CacheManager.GetStatistics().TotalSize;

                var sw = Stopwatch.StartNew();
                CacheManager.ClearAllCaches();
                sw.Stop();

                cacheScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.AllCleared,
                    "[Cache] Cache cleared successfully"));
                cacheScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.AllCleared,
                    $"[Cache] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));
                result.OperationResults = new[] { cacheScope };
            }
            catch (Exception e)
            {
                result.Successful = false;
                result.OperationResults = new[] { OperationResult.FromException(e) };
            }

            return result;
        }

        private OperationResult ClearDataCache(SiteContext site)
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

        private OperationResult ClearHtmlCache(SiteContext site)
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

        private OperationResult ClearItemCache(SiteContext site)
        {
            var itemScope = new OperationResult("Item");
            var size = site.Database.Caches.ItemCache.InnerCache.Size;

            var sw = Stopwatch.StartNew();
            site.Database.Caches.ItemCache.Clear();
            sw.Stop();

            itemScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.ItemCleared,
                "[Cache][Item] Item cache cleared successfully"));
            itemScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.ItemCleared,
                $"[Cache][Item] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));

            return itemScope;
        }

        private OperationResult ClearPathCache(SiteContext site)
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

        private OperationResult ClearItemPathsCache(SiteContext site)
        {
            var itemPathsScope = new OperationResult("ItemPaths");
            var size = site.Database.Caches.ItemPathsCache.InnerCache.Size;

            var sw = Stopwatch.StartNew();
            site.Database.Caches.ItemPathsCache.Clear();
            sw.Stop();

            itemPathsScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.ItemPathsCleared,
                "[Cache][ItemPaths] Item paths cache cleared successfully"));
            itemPathsScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.ItemPathsCleared,
                $"[Cache][ItemPaths] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));

            return itemPathsScope;
        }

        private OperationResult ClearStandardValuesCache(SiteContext site)
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

        private OperationResult ClearIsFallbackValidCache(SiteContext site)
        {
            var fallbackScope = new OperationResult("IsFallbackValid");
            var size = site.Database.Caches.IsFallbackValidCache.InnerCache.Size;

            var sw = Stopwatch.StartNew();
            site.Database.Caches.IsFallbackValidCache.Clear();
            sw.Stop();

            fallbackScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.IsFallbackValidCleared,
                "[Cache][IsFallbackValid] Is Fallback Valid values cache cleared successfully"));
            fallbackScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.IsFallbackValidCleared,
                $"[Cache][IsFallbackValid] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));

            return fallbackScope;
        }

        private OperationResult ClearRegistryCache(SiteContext site)
        {
            var registryScope = new OperationResult("Registry");
            var size = site.Caches.RegistryCache.InnerCache.Size;

            var sw = Stopwatch.StartNew();
            site.Caches.RegistryCache.Clear();
            sw.Stop();

            registryScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.RegistryCleared,
                "[Cache][Registry] Registry values cache cleared successfully"));
            registryScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.RegistryCleared,
                $"[Cache][Registry] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));

            return registryScope;
        }

        private OperationResult ClearXslCache(SiteContext site)
        {
            var xslScope = new OperationResult("Xsl");
            var size = site.Caches.XslCache.InnerCache.Size;

            var sw = Stopwatch.StartNew();
            site.Caches.XslCache.Clear();
            sw.Stop();

            xslScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.XslCleared,
                "[Cache][XSL] XSL cache cleared successfully"));
            xslScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.XslCleared,
                $"[Cache][XSL] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));

            return xslScope;
        }

        private OperationResult ClearFilteredItemsCache(SiteContext site)
        {
            var filteredItemsScope = new OperationResult("FilteredItems");
            var size = site.Caches.FilteredItemsCache.InnerCache.Size;

            var sw = Stopwatch.StartNew();
            site.Caches.FilteredItemsCache.Clear();
            sw.Stop();

            filteredItemsScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.FilteredItemsCleared,
                "[Cache][FilteredItems] Filtered Items cache cleared successfully"));
            filteredItemsScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.FilteredItemsCleared,
                $"[Cache][FilteredItems] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));

            return filteredItemsScope;
        }

        private OperationResult ClearRenderingParametersCache(SiteContext site)
        {
            var renderingParamsScope = new OperationResult("RenderingParameters");
            var size = site.Caches.RenderingParametersCache.InnerCache.Size;

            var sw = Stopwatch.StartNew();
            site.Caches.RenderingParametersCache.Clear();
            sw.Stop();

            renderingParamsScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.RenderingParametersCleared,
                "[Cache][RenderingParameters] Rendering Parameters cache cleared successfully"));
            renderingParamsScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.RenderingParametersCleared,
                $"[Cache][RenderingParameters] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));

            return renderingParamsScope;
        }

        private OperationResult ClearViewStateCache(SiteContext site)
        {
            var viewStateScope = new OperationResult("ViewState");
            var size = site.Caches.ViewStateCache.InnerCache.Size;

            var sw = Stopwatch.StartNew();
            site.Caches.ViewStateCache.Clear();
            sw.Stop();

            viewStateScope.Chain(OperationResult.FromInfoSuccess(CacheEventIds.ViewStateCleared,
                "[Cache][ViewState] View State cache cleared successfully"));
            viewStateScope.Chain(OperationResult.FromVerboseSuccess(CacheEventIds.ViewStateCleared,
                $"[Cache][ViewState] The {_bytesConverter.ToReadable(size)} cleared in {sw.ElapsedMilliseconds}ms"));

            return viewStateScope;
        }

        private IEnumerable<OperationResult> ClearAllCachesForSite(SiteContext site)
        {
            yield return ClearDataCache(site);
            yield return ClearHtmlCache(site);
            yield return ClearItemCache(site);
            yield return ClearPathCache(site);
            yield return ClearItemPathsCache(site);
            yield return ClearStandardValuesCache(site);
            yield return ClearIsFallbackValidCache(site);
            yield return ClearRegistryCache(site);
            yield return ClearXslCache(site);
            yield return ClearFilteredItemsCache(site);
            yield return ClearRenderingParametersCache(site);
            yield return ClearViewStateCache(site);
        }
    }
}