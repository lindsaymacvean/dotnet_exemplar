using System.ComponentModel;
using System.Text.Json.Serialization;

namespace dotnet_exemplar.Application.OpenAI.Models;

using System.ComponentModel;



public class MessageContent
{
    /// <summary>
    /// The type of the content message. E.g. "text"
    /// </summary>
    [DefaultValue("text")]
    [JsonPropertyName("type")]
    public string Type { get; set; } = "text";

    /// <summary>
    /// The actual message text.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}


public class ChatMessage
{
    /// <summary>
    /// The role of the message author. Defaults to "user" if not set.
    /// </summary>
    [DefaultValue("user")]
    [JsonPropertyName("role")]
    public string Role { get; set; } = "user";

    /// <summary>
    /// The content of the message, as an array of items (type/text objects).
    /// </summary>
    [JsonPropertyName("content")]
    public List<MessageContent> Content { get; set; } = new();
}


public class ChatCompletionsRequest
{
    [JsonPropertyName("messages")]
    public List<ChatMessage> Messages { get; set; } = new();

    /// <summary>
    /// Temperature for randomness, defaults to 0.7 if not supplied.
    /// </summary>
    [DefaultValue(0.7)]
    [JsonPropertyName("temperature")]
    public double? Temperature { get; set; } = 0.7;

    /// <summary>
    /// Top P value
    /// </summary>
    [DefaultValue(1.0)]
    [JsonPropertyName("top_p")]
    public double? TopP { get; set; } = 1.0;

    /// <summary>
    /// Maximum number of tokens, defaults to 1024 if not supplied.
    /// </summary>
    [DefaultValue(1024)]
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; } = 1024;
}