using Microsoft.Extensions.Logging;

namespace Sitecore.DevEx.Extensibility.Cache.Models
{
    public static class CacheEventIds
    {
        public static readonly EventId AllCleared = new EventId(1, nameof(DataCleared));
        public static readonly EventId DataCleared = new EventId(2, nameof(DataCleared));
        public static readonly EventId HtmlCleared = new EventId(3, nameof(HtmlCleared));
        public static readonly EventId ItemCleared = new EventId(4, nameof(ItemCleared));
        public static readonly EventId PathCleared = new EventId(5, nameof(PathCleared));
        public static readonly EventId ItemPathsCleared = new EventId(6, nameof(ItemPathsCleared));
        public static readonly EventId StandardValuesCleared = new EventId(7, nameof(StandardValuesCleared));
        public static readonly EventId IsFallbackValidCleared = new EventId(8, nameof(IsFallbackValidCleared));
        public static readonly EventId RegistryCleared = new EventId(9, nameof(RegistryCleared));
        public static readonly EventId XslCleared = new EventId(10, nameof(XslCleared));
        public static readonly EventId FilteredItemsCleared = new EventId(11, nameof(FilteredItemsCleared));
        public static readonly EventId RenderingParametersCleared = new EventId(12, nameof(RenderingParametersCleared));
        public static readonly EventId ViewStateCleared = new EventId(13, nameof(ViewStateCleared));
        
        public static readonly EventId ApiQuery = new EventId(128, nameof(PathCleared));
    }
}