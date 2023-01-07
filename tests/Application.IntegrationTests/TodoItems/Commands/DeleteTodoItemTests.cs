using FluentAssertions;
using MissBot.Common.Exceptions;
using MissBot.Domain.Entities;
using MissBot.TodoItems.Commands.CreateTodoItem;
using MissBot.TodoItems.Commands.DeleteTodoItem;
using MissBot.TodoLists.Commands.CreateTodoList;
using NUnit.Framework;
using static MissBot.Application.IntegrationTests.Testing;

namespace MissBot.Application.IntegrationTests.TodoItems.Commands;
public class DeleteTodoItemTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new DeleteTodoItemCommand(99);

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteTodoItem()
    {
        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        var itemId = await SendAsync(new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "New Item"
        });

        await SendAsync(new DeleteTodoItemCommand(itemId));

        var item = await FindAsync<TodoItem>(itemId);

        item.Should().BeNull();
    }
}
