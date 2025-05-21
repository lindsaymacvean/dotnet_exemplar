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

    public async Task<ChatCompletionsResponse> GetChatCompletionsAsync(
        ChatCompletionsRequest request,
        string deploymentId,
        CancellationToken cancellationToken)
    {
        
        var endpoint = $"{_endpoint.TrimEnd('/')}/deployments/{deploymentId}/chat/completions?api-version={_apiVersion}";

        var content = new StringContent(
        JsonSerializer.Serialize(request, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }),
        Encoding.UTF8,
        "application/json");

        _logger.LogInformation("Request content: {@Request}", request);
        _logger.LogInformation("Endpoint: {Endpoint}", endpoint);
        _logger.LogInformation("Api Key: {ApiKey}", _apiKey);

        _httpClient.DefaultRequestHeaders.Remove("api-key");
        _httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);

        var response = await _httpClient.PostAsync(endpoint, content, cancellationToken);
        _logger.LogInformation("Received response with status code: {StatusCode}", response.StatusCode);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("Error response from Azure OpenAI: {ErrorContent}", errorContent);
            throw new Exception($"Azure OpenAI API returned {response.StatusCode}: {errorContent}");
        }

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        _logger.LogInformation("Response content: {ResponseContent}", responseContent);

        var result = JsonSerializer.Deserialize<ChatCompletionsResponse>(responseContent);
        if (result == null)
        {
            _logger.LogError("Failed to deserialize response content");
            throw new Exception("Failed to deserialize response from Azure OpenAI");
        }

        return result;
    }
}