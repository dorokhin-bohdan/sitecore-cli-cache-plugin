namespace Sitecore.DevEx.Extensibility.Cache.Api.Services
{
    public interface IBytesConverter
    {
        string ToReadable(long value, int decimals = 2);
    }
}