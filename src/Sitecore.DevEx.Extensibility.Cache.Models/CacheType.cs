using System;

namespace Sitecore.DevEx.Extensibility.Cache.Models
{
    [Flags]
    public enum CacheType
    {
        Data = 1,
        Html = 2,
        Item = 4,
        Path = 8,
        ItemPaths = 16,
        StandardValues = 32,
        IsFallbackValid = 64,
        Registry = 128,
        Xsl = 256,
        FilteredItems = 512,
        RenderingParameters = 1024,
        ViewState = 2048
    }
}