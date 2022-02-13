using Sitecore.Caching;
using Sitecore.Data;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Tests.Fakes
{
    public class FakePathCache : PathCache
    {
        public FakePathCache(Database database, long maxSize, ICache innerCache) : base(database, maxSize)
        {
            InnerCache = innerCache;
        }
    }
}