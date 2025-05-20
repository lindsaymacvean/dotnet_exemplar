using MediatR;

namespace dotnet_exemplar.Application.OpenAI.Queries;

public class ValidateApiTokenQuery : IRequest<bool>
{
    public string ApiToken { get; }

    public ValidateApiTokenQuery(string apiToken)
    {
        ApiToken = apiToken;
    }
}