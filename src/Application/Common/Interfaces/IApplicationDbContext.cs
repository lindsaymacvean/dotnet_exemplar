using dotnet_exemplar.Domain.Entities;

namespace dotnet_exemplar.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<ApiToken> ApiTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
