using MissBot.TodoLists.Queries.ExportTodos;

namespace MissBot.Common.Interfaces;
public interface ICsvFileBuilder
{
    byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
}
