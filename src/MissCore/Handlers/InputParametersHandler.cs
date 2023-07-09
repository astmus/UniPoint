using MissBot.Abstractions;
using MissBot.Abstractions.Bot;
using MissBot.Entities;
using MissBot.Entities.API;

namespace MissCore.Handlers
{
	public class InputParametersHandler : BotUnitActionHadlerBase<BaseAction>
	{
		protected override void Initialize(IEnumerable<string> parameterNames)
		{
			if (currentUnit.ArgumentCount == 0) return;
			var args = Enumerable.Repeat<AsyncInputHandler>(SetParameter, currentUnit.ArgumentCount).ToArray();
			JoinHandlers(args);
		}

		async Task Save(IHandleContext context)
		{
			var listCmds = context.Bot.Commands.ToList();
			//listCmds.Add(add);
			var scope = BotCommandScope.Chat(context.Get<Chat>().Id);
			var success = await context.BotServices.Client.SyncCommandsAsync(listCmds, scope);
			if (success)
			{
				//context.Bot.Commands.Add(currentUnit);
				Response.CompleteInteraction("Command added");
			}
			else
				Response.CompleteInteraction("Save failed");

			context.IsHandled = true;
		}

		Task Cancel(IHandleContext context)
		{
			Response.CompleteInteraction("Operation cancelled");
			context.IsHandled = true;
			return Task.CompletedTask;
		}

		object SetParameter(IHandleContext context, string input, string parameterName) => input switch
		{
			null when currentUnit[parameterName] is null =>
				Response.InputDataInteraction($"Enter parameter: '{parameterName}'"),
			var value when value.IndexOf(' ') < 0 =>
				currentUnit[parameterName] = value,
			var value when value.IndexOf(' ') > -1 =>
				ReTry(SetParameter, "Invalid parameter format"),
			_ => null
		};
	}
}
