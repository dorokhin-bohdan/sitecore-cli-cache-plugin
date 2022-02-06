using System.Web.Http;
using Sitecore.DevEx.Extensibility.Cache.Api.Services;
using Sitecore.DevEx.Extensibility.Cache.Models.Requests;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Controllers
{
    [Authorize]
    public class CacheCleanupController : ApiController
    {
        private readonly ICacheService _cacheService;

        public CacheCleanupController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpPost]
        [Route("sitecore/api/cachecleanup/site")]
        public IHttpActionResult ClearSiteCache(CacheCleanupRequest request)
        {
            var result = _cacheService.ClearBySite(request.Site, request.CacheType);
            return Json(result);
        }

        [HttpPost]
        [Route("sitecore/api/cachecleanup")]
        public IHttpActionResult ClearAllCache()
        {
            var result = _cacheService.ClearAll();
            return Json(result);
        }
    }
}