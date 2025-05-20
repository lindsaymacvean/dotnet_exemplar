using System.ComponentModel;

namespace dotnet_exemplar.Application.OpenAI.Models;

using System.ComponentModel;

public class ChatMessage {
    /// <summary>
    /// The role of the message author. Defaults to "user" if not set.
    /// </summary>
    [DefaultValue("user")]
    public string Role { get; set; } = "user";

    /// <summary>
    /// The content of the message. Defaults to empty string.
    /// </summary>
    [DefaultValue("Tell me how amazing I am.")]
    public string Content { get; set; } = string.Empty;
}

public class ChatCompletionsRequest {

    public List<ChatMessage> Messages { get; set; } = new();

    /// <summary>
    /// Temperature for randomness, defaults to 0.7 if not supplied.
    /// </summary>
    [DefaultValue(0.7)]
    public double? Temperature { get; set; } = 0.7;

    /// <summary>
    /// Maximum number of tokens, defaults to 1024 if not supplied.
    /// </summary>
    [DefaultValue(1024)]
    public int? MaxTokens { get; set; } = 1024;
}