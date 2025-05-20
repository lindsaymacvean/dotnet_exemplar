namespace dotnet_exemplar.Application.OpenAI.Models;

public class ChatCompletionsResponse
{
    public string Id { get; set; } = string.Empty;
    public string Object { get; set; } = "chat.completion";
    public long Created { get; set; }
    public string Model { get; set; } = string.Empty;
    public List<ChatChoice> Choices { get; set; } = new();
}

public class ChatChoice
{
    public int Index { get; set; }
    public ChatMessage Message { get; set; } = new ChatMessage();
    public string FinishReason { get; set; } = "stop";
}