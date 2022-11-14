using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Sitecore.DevEx.Extensibility.Cache.Services;

public class JsonService : IJsonService
{
    private readonly JsonSerializerSettings _settings = new()
    {
        Formatting = Formatting.Indented,
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        TypeNameHandling = TypeNameHandling.None
    };
        
    public string Serialize<T>(T obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        return JsonConvert.SerializeObject(obj, _settings);
    }

    public T Deserialize<T>(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new ArgumentNullException(nameof(json));
        }
            
        return JsonConvert.DeserializeObject<T>(json, _settings);
    }
}