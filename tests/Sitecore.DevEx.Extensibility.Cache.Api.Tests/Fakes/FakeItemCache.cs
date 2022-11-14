using Sitecore.Caching;
using Sitecore.Caching.Generics;
using Sitecore.Data;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Tests.Fakes;

public class FakeItemCache : ItemCache
{
    public FakeItemCache(Database database, long maxSize, ICache<string> innerCache) : base(database, maxSize)
    {
        InnerCache = innerCache;
    }
}