using MissBot.Common.Mappings;
using MissBot.Domain.Entities;

namespace MissBot.TodoLists.Queries.ExportTodos;
public class TodoItemRecord : IMapFrom<TodoItem>
{
    public string? Title { get; set; }

    public bool Done { get; set; }
}
