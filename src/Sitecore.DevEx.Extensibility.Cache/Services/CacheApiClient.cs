using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sitecore.DevEx.Client.Logging;
using Sitecore.DevEx.Configuration.Models;
using Sitecore.DevEx.Extensibility.Cache.Constants;
using Sitecore.DevEx.Extensibility.Cache.Models;
using Sitecore.DevEx.Extensibility.Cache.Models.Requests;

namespace Sitecore.DevEx.Extensibility.Cache.Services;

public class CacheApiClient : ICacheApiClient
{
    private readonly ILogger<CacheApiClient> _logger;
    private readonly IJsonService _jsonService;
    private readonly IHttpClientFactory _httpClientFactory;

    public CacheApiClient(IHttpClientFactory httpClientFactory, IJsonService jsonService,
        ILogger<CacheApiClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _jsonService = jsonService;
        _logger = logger;
    }

    public async Task<CacheResultModel> ClearBySiteAsync(EnvironmentConfiguration configuration,
        CacheCleanupRequest request)
    {
        const string apiPath = "sitecore/api/cachecleanup/site";
        _logger.LogConsoleInformation("Processing...", ConsoleColor.Yellow);
        _logger.LogTrace(CacheEventIds.ApiQuery, "Sending cache request...");

        var client = GetHttpClient(configuration);
        var json = _jsonService.Serialize(request);
        try
        {
            var response = await configuration.MakeAuthenticatedRequest(client,
                    _ => client.PostAsync(apiPath, new StringContent(json, Encoding.UTF8, "application/json")))
                .ConfigureAwait(false);
            var resultJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return _jsonService.Deserialize<CacheResultModel>(resultJson);
        }
        catch (Exception e)
        {
            _logger.LogTrace(CacheEventIds.ApiQuery,
                $"HTTP Error invoking API call {configuration.Host}{apiPath}.");
            _logger.LogConsole(LogLevel.Error, e.Message);
        }

        return null;
    }

    public async Task<CacheResultModel> ClearAllAsync(EnvironmentConfiguration configuration)
    {
        const string apiPath = "sitecore/api/cachecleanup/global";
        _logger.LogConsoleInformation("Processing...", ConsoleColor.Yellow);
        _logger.LogTrace(CacheEventIds.ApiQuery, "Sending cache request...");

        var client = GetHttpClient(configuration);
        try
        {
            var response = await configuration
                .MakeAuthenticatedRequest(client, _ => client.PostAsync(apiPath, null))
                .ConfigureAwait(false);
            var resultJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return _jsonService.Deserialize<CacheResultModel>(resultJson);
        }
        catch (Exception e)
        {
            _logger.LogTrace(CacheEventIds.ApiQuery,
                $"HTTP Error invoking API call {configuration.Host}{apiPath}.");
            _logger.LogConsole(LogLevel.Error, e.Message);
        }
            
        return null;
    }

    private HttpClient GetHttpClient(EnvironmentConfiguration configuration)
    {
        var httpClient = _httpClientFactory.CreateClient(CacheConstants.HttpClientName);
        httpClient.BaseAddress = configuration.Host;
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return httpClient;
    }
}