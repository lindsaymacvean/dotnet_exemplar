using dotnet_exemplar.Domain.Events;
using Microsoft.Extensions.Logging;

namespace dotnet_exemplar.Application.TodoItems.EventHandlers;

public class TodoItemCompletedEventHandler : INotificationHandler<TodoItemCompletedEvent>
{
    private readonly ILogger<TodoItemCompletedEventHandler> _logger;

    public TodoItemCompletedEventHandler(ILogger<TodoItemCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TodoItemCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("dotnet_exemplar Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
