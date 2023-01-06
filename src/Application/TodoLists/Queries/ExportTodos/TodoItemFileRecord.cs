using MissBot.Application.Common.Mappings;
using MissBot.Domain.Entities;

namespace MissBot.Application.TodoLists.Queries.ExportTodos;
public class TodoItemRecord : IMapFrom<TodoItem>
{
    public string? Title { get; set; }

    public bool Done { get; set; }
}
