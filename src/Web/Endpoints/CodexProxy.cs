using Microsoft.AspNetCore.Mvc;
using dotnet_exemplar.Application.OpenAI.Models;
using dotnet_exemplar.Application.OpenAI.Queries;
using dotnet_exemplar.Application.Common.Interfaces;
using System.Text.Json;

namespace dotnet_exemplar.Web.Endpoints;

/// <summary>
/// OpenAI mimicked chat completions API controller
/// </summary>
[ApiController] // model validation/400, automatic binding, openapi generation
[Route("openai/deployments/{deploymentId}/chat/completions")]
public class CodexProxy : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IChatService _chatService;
    private readonly ILogger<CodexProxy> _logger;

    public CodexProxy(IMediator mediator, IChatService chatService, ILogger<CodexProxy> logger)
    {
        _mediator = mediator;
        _chatService = chatService;
        _logger = logger;
    }

    /// <summary>
    /// Chat Completions endpoint that mimics Azure OpenAI.
    /// </summary>
    /// <param name="deploymentId">Deployment/model ID</param>
    /// <param name="apiVersion">API version. Must be 2025-01-01-preview</param>
    /// <param name="apiKey">Client API key issued by this service. <b>Not the Azure service API key!</b> Use the default pre-filled key for local/dev.<br/>EXAMPLE: <code>f6e4d7fe-8e76-4d8c-bf76-23c7fad51eab</code></param>
    /// <param name="request">Chat completions request body</param>
    /// <param name="cancellationToken">Cancellation</param>
    /// <returns>OpenAI chat response</returns>
    [HttpPost]
    [ProducesResponseType(typeof(dotnet_exemplar.Application.OpenAI.Models.ChatCompletionsResponse), 200)]
    [ProducesResponseType(typeof(object), 400)]
    [ProducesResponseType(typeof(object), 401)]
    public async Task<IActionResult> ChatCompletions(
        [FromHeader(Name = "api-key")] string apiKey,
        [FromRoute] string deploymentId,
        [FromBody] System.Text.Json.JsonElement rawPayload,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("ChatCompletions endpoint called with deploymentId: {DeploymentId}", deploymentId);
        _logger.LogInformation("Raw payload: {RawPayload}", rawPayload);
        _logger.LogInformation("API key: {ApiKey}", apiKey);
        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogWarning("Missing API key");
            return Unauthorized(new { error = "Missing or empty api-key header" });
        }
        bool valid = await _mediator.Send(new ValidateApiTokenQuery(apiKey), cancellationToken);
        if (!valid)
        {
            return Unauthorized(new { error = "Invalid or revoked api-key" });
        }
        try
        {
            string payloadString = rawPayload.GetRawText();
            var result = await _chatService.GetChatCompletionsAsync(payloadString, deploymentId, cancellationToken);
            return Content(result, "application/json");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error proxying chat completions");
            return Problem(ex.Message);
        }
    }
}
