using Sitecore.Data;
using Sitecore.Sites;
using Sitecore.Web;

namespace Sitecore.DevEx.Extensibility.Cache.Api.Tests.Fakes;

public class FakeSiteContext : SiteContext
{
    public FakeSiteContext(SiteInfo info) : base(info)
    {
    }
        
    public FakeSiteContext(SiteInfo info, Database database) : base(info)
    {
        Database = database;
    }
}