using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sitecore.DevEx.Configuration.Models;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Extensibility.Cache.Models.Requests;

namespace Sitecore.DevEx.Extensibility.Cache.Services
{
    public class CacheApiClient : ICacheApiClient
    {
        private readonly ILogger<CacheApiClient> _logger;
        private readonly IJsonService _jsonService;
        private readonly IHttpClientFactory _httpClientFactory;

        public CacheApiClient(IHttpClientFactory httpClientFactory, IJsonService jsonService, ILogger<CacheApiClient> logger)
        {
            _httpClientFactory = httpClientFactory;
            _jsonService = jsonService;
            _logger = logger;
        }

        public async Task<CacheResultModel> ClearBySiteAsync(EnvironmentConfiguration configuration, CacheCleanupRequest request)
        {
            var client = GetHttpClient(configuration);
            var json = _jsonService.Serialize(request);
            var response = await client.PostAsync("sitecore/api/cachecleanup/site", new StringContent(json)).ConfigureAwait(false);
            var resultJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return _jsonService.Deserialize<CacheResultModel>(resultJson);
        }
        
        public async Task<CacheResultModel> ClearAllAsync(EnvironmentConfiguration configuration)
        {
            var client = GetHttpClient(configuration);
            var response = await client.PostAsync("sitecore/api/cachecleanup", null).ConfigureAwait(false);
            var resultJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return _jsonService.Deserialize<CacheResultModel>(resultJson);
        }

        private HttpClient GetHttpClient(EnvironmentConfiguration configuration)
        {
            var httpClient = _httpClientFactory.CreateClient("Cache");
            httpClient.BaseAddress = configuration.Host;
            
            httpClient.SetBearerToken(configuration.AccessToken);
            
            return httpClient;
        }
    }
}