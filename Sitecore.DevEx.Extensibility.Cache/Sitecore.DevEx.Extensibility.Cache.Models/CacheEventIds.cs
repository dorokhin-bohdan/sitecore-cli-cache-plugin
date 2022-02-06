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
        
        public static readonly EventId ApiQuery = new EventId(128, nameof(PathCleared));
    }
}