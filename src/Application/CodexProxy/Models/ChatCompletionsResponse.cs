namespace dotnet_exemplar.Application.OpenAI.Models;

public class ChatCompletionsResponse
{
    public string Id { get; set; } = string.Empty;
    public string Object { get; set; } = "chat.completion";
    public long Created { get; set; }
    public string Model { get; set; } = string.Empty;
    public List<ChatChoice> Choices { get; set; } = new();
    public List<PromptFilterResult> PromptFilterResults { get; set; } = new();
    public string SystemFingerprint { get; set; } = string.Empty;
    public UsageInfo? Usage { get; set; }
}

public class ChatChoice
{
    public int Index { get; set; }
    public ChoiceContentFilterResults? ContentFilterResults { get; set; }
    public string? FinishReason { get; set; }
    public object? Logprobs { get; set; }
    public ChoiceMessage Message { get; set; } = new();
}

public class ChoiceContentFilterResults
{
    public ModerationResult? Hate { get; set; }
    public ModerationResult? SelfHarm { get; set; }
    public ModerationResult? Sexual { get; set; }
    public ModerationResult? Violence { get; set; }
    public ProtectedMaterialFilterResult? ProtectedMaterialText { get; set; }
    public ProtectedMaterialFilterResult? ProtectedMaterialCode { get; set; }
}

public class ModerationResult
{
    public bool Filtered { get; set; }
    public string Severity { get; set; } = string.Empty;
}
public class ProtectedMaterialFilterResult
{
    public bool Filtered { get; set; }
    public bool Detected { get; set; }
}

public class ChoiceMessage
{
    public List<object>? Annotations { get; set; }
    public string Content { get; set; } = string.Empty;
    public object? Refusal { get; set; }
    public string Role { get; set; } = string.Empty;
}

public class PromptFilterResult
{
    public int PromptIndex { get; set; }
    public Dictionary<string, object>? ContentFilterResults { get; set; }
}

public class UsageInfo
{
    public int CompletionTokens { get; set; }
    public int PromptTokens { get; set; }
    public int TotalTokens { get; set; }
    public CompletionTokensDetails? CompletionTokensDetails { get; set; }
    public PromptTokensDetails? PromptTokensDetails { get; set; }
}

public class CompletionTokensDetails
{
    public int AcceptedPredictionTokens { get; set; }
    public int AudioTokens { get; set; }
    public int ReasoningTokens { get; set; }
    public int RejectedPredictionTokens { get; set; }
}
public class PromptTokensDetails
{
    public int AudioTokens { get; set; }
    public int CachedTokens { get; set; }
}