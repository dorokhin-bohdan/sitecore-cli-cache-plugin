using System;

namespace Sitecore.DevEx.Extensibility.Cache.Models
{
    [Flags]
    public enum CacheType
    {
        Data = 1,
        Html = 2,
        Item = 4,
        Path = 8
    }
}