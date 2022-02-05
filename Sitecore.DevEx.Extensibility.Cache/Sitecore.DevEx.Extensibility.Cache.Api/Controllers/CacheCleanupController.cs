using System.Web.Http;
using Sitecore.DevEx.Extensibility.Cache.Api.Services;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Extensibility.Cache.Models.Requests;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Controllers
{
    [Authorize]
    [RoutePrefix("sitecore/api/cachecleanup")]
    public class CacheCleanupController : ApiController
    {
        private readonly ICacheService _cacheService;
        
        public CacheCleanupController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        
        [HttpPost]
        [Route("site")]
        public IHttpActionResult ClearSiteCache(CacheCleanupRequest request)
        {
            _cacheService.ClearBySite(request.Site, request.CacheType);
            
            return Json(new CacheResultModel
            {
                Successful = true
            });
        }
        
        [HttpPost]
        [Route()]
        public IHttpActionResult ClearAllCache()
        {
            _cacheService.ClearAll();
            
            return Json(new CacheResultModel
            {
                Successful = true
            });
        }
    }
}