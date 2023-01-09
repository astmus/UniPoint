using MediatR;
using MissCore.DataAccess;

namespace MissBot.TodoItems.Commands.CreateTodoItem;
public record CreateTodoItemCommand : IRequest<int>
{
    public int ListId { get; init; }

    public string? Title { get; init; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, int>
{
    private readonly IApplicationGenericRepository _context;

    public CreateTodoItemCommandHandler(IApplicationGenericRepository context)
    {
        _context = context;
    }

    public Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        //var entity = new TodoItem
        //{
        //    ListId = request.ListId,
        //    Title = request.Title,
        //    Done = false
        //};

        //entity.AddDomainEvent(new TodoItemCreatedEvent(entity));

        //_context.TodoItems.Add(entity);

        //await _context.SaveChangesAsync(cancellationToken);

        //return entity.Id;
        return Task.FromResult(0);
    }
}
