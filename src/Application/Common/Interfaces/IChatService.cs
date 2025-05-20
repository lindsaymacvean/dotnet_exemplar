using dotnet_exemplar.Application.OpenAI.Models;

namespace dotnet_exemplar.Application.Common.Interfaces;

public interface IChatService
{
    Task<ChatCompletionsResponse> GetChatCompletionsAsync(ChatCompletionsRequest request, string deploymentId, CancellationToken cancellationToken);
}