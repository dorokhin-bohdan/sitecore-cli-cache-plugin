using Sitecore.Caching;
using Sitecore.Caching.Generics;
using Sitecore.Data;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Tests.Fakes;

public class FakeDataCache : DataCache
{
    public FakeDataCache(Database database, long maxSize, ICache<ID> innerCache) : base(database, maxSize)
    {
        InnerCache = innerCache;
    }
}