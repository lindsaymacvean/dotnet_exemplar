using MediatR;
using Microsoft.AspNetCore.Mvc;
using dotnet_exemplar.Application.OpenAI.Models;
using dotnet_exemplar.Application.OpenAI.Queries;
using dotnet_exemplar.Application.Common.Interfaces;

namespace dotnet_exemplar.Web.Endpoints;

/// <summary>
/// OpenAI mimicked chat completions API controller
/// </summary>
[ApiController]
[Route("openai/deployments/{deploymentId}/chat/completions")]
public class CodexProxy : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IChatService _chatService;

    public CodexProxy(IMediator mediator, IChatService chatService)
    {
        _mediator = mediator;
        _chatService = chatService;
    }

    /// <summary>
    /// Chat Completions endpoint that mimics Azure OpenAI.
    /// </summary>
    /// <param name="deploymentId">Deployment/model ID</param>
    /// <param name="apiVersion">API version. Must be 2025-01-01-preview</param>
    /// <param name="httpContextAccessor">For header capture</param>
    /// <param name="request">Chat completions request body</param>
    /// <param name="cancellationToken">Cancellation</param>
    /// <returns>OpenAI chat response</returns>
    [HttpPost]
    [ProducesResponseType(typeof(dotnet_exemplar.Application.OpenAI.Models.ChatCompletionsResponse), 200)]
    [ProducesResponseType(typeof(object), 400)]
    [ProducesResponseType(typeof(object), 401)]
    public async Task<IActionResult> ChatCompletions(
        [FromRoute] string deploymentId,
        [FromQuery(Name = "api-version")] string apiVersion,
        [FromServices] IHttpContextAccessor httpContextAccessor,
        [FromBody] ChatCompletionsRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(apiVersion) ||
            apiVersion != "2025-01-01-preview")
        {
            return BadRequest(new { error = "api-version must be 2025-01-01-preview" });
        }

        // Validate api-token header
        var headers = httpContextAccessor.HttpContext?.Request.Headers;
        if (headers == null || !headers.TryGetValue("api-token", out var tokenHeader) || string.IsNullOrWhiteSpace(tokenHeader))
        {
            return Unauthorized(new { error = "Missing or empty api-token header" });
        }
        string apiToken = tokenHeader!;

        // Call application layer to validate
        bool valid = await _mediator.Send(new ValidateApiTokenQuery(apiToken), cancellationToken);
        if (!valid)
        {
            return Unauthorized(new { error = "Invalid or revoked API token" });
        }

        // Call chat service for completion
        var response = await _chatService.GetChatCompletionsAsync(request, deploymentId, cancellationToken);
        // Ensure the response includes a model (fallback if missing)
        if (string.IsNullOrWhiteSpace(response.Model))
        {
            response.Model = "gpt-4.1";
        }
        return Ok(response);
    }
}
