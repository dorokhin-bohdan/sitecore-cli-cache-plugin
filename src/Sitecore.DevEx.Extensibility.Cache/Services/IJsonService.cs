namespace Sitecore.DevEx.Extensibility.Cache.Services;

public interface IJsonService
{
    string Serialize<T>(T obj);
        
    T Deserialize<T>(string json);
}