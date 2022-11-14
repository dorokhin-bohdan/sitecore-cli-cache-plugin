using Microsoft.Extensions.Logging;

namespace Sitecore.DevEx.Extensibility.Cache.Models;

public static class CacheEventIds
{
    public static readonly EventId AllCleared = new(1, nameof(AllCleared));
    public static readonly EventId DataCleared = new(2, nameof(DataCleared));
    public static readonly EventId HtmlCleared = new(3, nameof(HtmlCleared));
    public static readonly EventId ItemCleared = new(4, nameof(ItemCleared));
    public static readonly EventId PathCleared = new(5, nameof(PathCleared));
    public static readonly EventId ItemPathsCleared = new(6, nameof(ItemPathsCleared));
    public static readonly EventId StandardValuesCleared = new(7, nameof(StandardValuesCleared));
    public static readonly EventId IsFallbackValidCleared = new(8, nameof(IsFallbackValidCleared));
    public static readonly EventId RegistryCleared = new(9, nameof(RegistryCleared));
    public static readonly EventId XslCleared = new(10, nameof(XslCleared));
    public static readonly EventId FilteredItemsCleared = new(11, nameof(FilteredItemsCleared));
    public static readonly EventId RenderingParametersCleared = new(12, nameof(RenderingParametersCleared));
    public static readonly EventId ViewStateCleared = new(13, nameof(ViewStateCleared));
        
    public static readonly EventId ApiQuery = new(128, nameof(ApiQuery));
}