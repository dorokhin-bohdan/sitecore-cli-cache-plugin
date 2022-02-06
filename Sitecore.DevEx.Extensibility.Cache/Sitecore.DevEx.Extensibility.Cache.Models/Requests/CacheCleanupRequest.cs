namespace Sitecore.DevEx.Extensibility.Cache.Models.Requests
{
    public class CacheCleanupRequest
    {
        public CacheType? CacheType { get; set; }
        public string Site { get; set; }
    }
}