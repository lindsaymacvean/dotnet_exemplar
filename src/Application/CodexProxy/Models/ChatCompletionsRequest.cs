namespace dotnet_exemplar.Application.OpenAI.Models;

public class ChatMessage {
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

public class ChatCompletionsRequest {
    public List<ChatMessage> Messages { get; set; } = new();
    public double? Temperature { get; set; }
    public int? MaxTokens { get; set; }
}