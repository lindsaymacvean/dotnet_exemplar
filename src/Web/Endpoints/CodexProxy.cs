using MediatR;
using Microsoft.AspNetCore.Mvc;
using dotnet_exemplar.Application.OpenAI.Models;
using dotnet_exemplar.Application.OpenAI.Queries;
using dotnet_exemplar.Application.Common.Interfaces;
using dotnet_exemplar.Application.Common.Models;
using System.Text.Json;
using Microsoft.Extensions.Logging;

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
        [FromBody] ChatCompletionsRequest request,
        CancellationToken cancellationToken,
        [FromRoute, System.ComponentModel.DefaultValue("gpt-4")] string deploymentId = "gpt-4",
        [FromQuery(Name = "api-version"), System.ComponentModel.DefaultValue("2025-01-01-preview")] string apiVersion = "2025-01-01-preview")
    {
        var apiKey = Request.Headers["api-key"].FirstOrDefault();
        _logger.LogInformation("Received chat completion request for deployment {DeploymentId}", deploymentId);
        _logger.LogInformation("API Version: {ApiVersion}", apiVersion);
        _logger.LogInformation("Request body: {@Request}", request);

        if (string.IsNullOrEmpty(apiVersion) || apiVersion != "2025-01-01-preview")
        {
            _logger.LogWarning("Invalid API version: {ApiVersion}", apiVersion);
            return BadRequest(new { error = "Invalid API version" });
        }

        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogWarning("Missing API key");
            return Unauthorized(new { error = "Missing or empty api-key header" });
        }

        // Call application layer to validate
        bool valid = await _mediator.Send(new ValidateApiTokenQuery(apiKey), cancellationToken);
        if (!valid)
        {
            return Unauthorized(new { error = "Invalid or revoked api-key" });
        }

        try
        {
            // Call chat service for completion
            var response = await _chatService.GetChatCompletionsAsync(request, deploymentId, cancellationToken);
            _logger.LogInformation("Generated response: {@Response}", response);
            // Ensure the response includes a model (fallback if missing)
            if (string.IsNullOrWhiteSpace(response.Model))
            {
                response.Model = "gpt-4.1";
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating chat completion");
            return Problem(ex.Message);
        }
    }
}
