using System;

namespace Sitecore.DevEx.Extensibility.Cache.Models
{
    [Flags]
    public enum CacheType
    {
        All = 0x0b,
        Data = 0x1b,
        Html = 0x10b,
        Item = 0x100b,
        Path = 0x1000b
    }
}