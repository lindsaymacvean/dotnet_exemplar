using dotnet_exemplar.Application.OpenAI.Models;

namespace dotnet_exemplar.Application.Common.Interfaces;

public interface IChatService
{
    /// <summary>Proxies a raw (JSON) chat completions request to the AI service and returns the raw response.</summary>
    Task<string> GetChatCompletionsAsync(string rawPayload, string deploymentId, CancellationToken cancellationToken);
}