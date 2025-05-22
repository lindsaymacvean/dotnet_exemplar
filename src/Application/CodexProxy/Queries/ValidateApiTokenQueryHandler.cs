using dotnet_exemplar.Application.Common.Interfaces;

namespace dotnet_exemplar.Application.OpenAI.Queries;

public class ValidateApiTokenQueryHandler : IRequestHandler<ValidateApiTokenQuery, bool>
{
    private readonly IApplicationDbContext _context;

    public ValidateApiTokenQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ValidateApiTokenQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.ApiToken))
            return false;
        // validate against tokens (not revoked)
        return await _context.ApiTokens.AnyAsync(
            x => x.Key == request.ApiToken && !x.IsRevoked, cancellationToken);
    }
}