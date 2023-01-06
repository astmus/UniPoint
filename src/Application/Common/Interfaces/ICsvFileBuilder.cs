using MissBot.Application.TodoLists.Queries.ExportTodos;

namespace MissBot.Application.Common.Interfaces;
public interface ICsvFileBuilder
{
    byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}
