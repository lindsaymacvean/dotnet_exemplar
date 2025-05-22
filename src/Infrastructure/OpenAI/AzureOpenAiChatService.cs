using System.Text;
using System.Text.Json;
using dotnet_exemplar.Application.Common.Interfaces;
using dotnet_exemplar.Application.OpenAI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace dotnet_exemplar.Infrastructure.OpenAI;

public class AzureOpenAiChatService : IChatService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AzureOpenAiChatService> _logger;
    private readonly string _endpoint;
    private readonly string _apiKey;
    private readonly string _apiVersion = "2023-03-15-preview";

    public AzureOpenAiChatService(HttpClient httpClient, IConfiguration configuration, ILogger<AzureOpenAiChatService> logger)
    //public AzureOpenAiChatService(HttpClient httpClient, IConfiguration configuration, ILogger<AzureOpenAiChatService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
        _endpoint = _configuration["AzureOpenAI:Endpoint"] ?? throw new InvalidOperationException("AzureOpenAI:Endpoint not configured");
        _apiKey = _configuration["AzureOpenAI:ApiKey"] ?? throw new InvalidOperationException("AzureOpenAI:ApiKey not configured");
    }

    public async Task<string> GetChatCompletionsAsync(
        string rawPayload,
        string deploymentId,
        CancellationToken cancellationToken)
    {
        var endpoint = $"{_endpoint.TrimEnd('/')}/deployments/{deploymentId}/chat/completions?api-version={_apiVersion}";
        var content = new StringContent(rawPayload, Encoding.UTF8, "application/json");

        _logger.LogInformation("[AzureOpenAiChatService] Outbound endpoint: {Endpoint}", endpoint);
        _logger.LogInformation("[AzureOpenAiChatService] Outbound payload: {Payload}", rawPayload);

        // Ensure proper headers
        _httpClient.DefaultRequestHeaders.Remove("api-key");
        _httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);

        // Explicitly provide model header (not normally needed but Azure error says it's supported)
        _httpClient.DefaultRequestHeaders.Remove("x-ms-model-mesh-model-name");
        _httpClient.DefaultRequestHeaders.Add("x-ms-model-mesh-model-name", deploymentId);

        var response = await _httpClient.PostAsync(endpoint, content, cancellationToken);
        _logger.LogInformation("[AzureOpenAiChatService] Received status: {StatusCode}", response.StatusCode);

        string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("[AzureOpenAiChatService] Error response: {ErrorContent}", responseContent);
            throw new Exception($"Azure OpenAI API returned {response.StatusCode}: {responseContent}");
        }
        //_logger.LogInformation("[AzureOpenAiChatService] Raw Azure response: {ResponseContent}", responseContent);
        return responseContent;
    }
}