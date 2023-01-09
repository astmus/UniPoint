using MediatR;
using MissBot.Common.Interfaces;

namespace MissBot.TodoLists.Commands.CreateTodoList;
public record CreateTodoListCommand : IRequest<int>
{
    public string? Title { get; init; }
}

public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoListCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<int> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        //var entity = new TodoList();

        //entity.Title = request.Title;

        //_context.TodoLists.Add(entity);

        //await _context.SaveChangesAsync(cancellationToken);

        //return entity.Id;
        return Task.FromResult(1);
    }
}
