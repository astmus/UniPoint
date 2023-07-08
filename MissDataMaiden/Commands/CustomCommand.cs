using System.Diagnostics.CodeAnalysis;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Bot;
using MissBot.Entities;
using MissBot.Entities.API;
using MissCore.Bot;
using MissCore.Handlers;

namespace MissDataMaiden.Commands
{
	public record CustomCommand : BotUnitCommand
	{
		[NotNull]
		public string Aliase { get => Action; set => Action = value; }
		public string Request { get => Template; set => Template = value; }
	}

	public class CustomCommandCreateHandler : BotUnitActionHadlerBase<CustomCommand>
	{
		private readonly IRepository<BotCommand> repository;

		CustomCommand currentCommand;
		protected override void Initialize(IEnumerable<string> parameterNames)
		{
			currentCommand = new CustomCommand();
			JoinHandlers(SetAliase, SetDescription, SetCommand, OfferCompleteOptions);
		}

		object OfferCompleteOptions(IHandleContext context, string input, string parameterName) => input switch
		{
			null => Response.InputDataInteraction("Save command?", ChatActions.Create(1, nameof(Save), nameof(Cancel))),
			nameof(Save) => Save(context),
			nameof(Cancel) => Cancel(context),
			_ => null
		};

		async Task Save(IHandleContext context)
		{
			var listCmds = context.Bot.Commands.ToList();
			listCmds.Add(currentCommand);
			var success = await context.BotServices.Client.SyncCommandsAsync(listCmds, BotCommandScope.Chat(context.Get<Chat>().Id));
			if (success)
			{
				context.Bot.Commands.Add(currentCommand);
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

		object SetAliase(IHandleContext context, string input, string parameterName) => input switch
		{
			null =>
				Response.InputDataInteraction("Enter unique command alise"),
			var value when value.IndexOf(' ') < 0 =>
				currentCommand.Aliase = value,
			var value when value.IndexOf(' ') > -1 =>
				ReTry(SetAliase, "Invalid aliase format"),
			_ => null
		};
		object SetDescription(IHandleContext context, string input, string parameterName) => input switch
		{
			null => Response.InputDataInteraction("Enter command description"),
			_ => currentCommand.Description = input
		};
		object SetCommand(IHandleContext context, string input, string parameterName) => input switch
		{
			null => Response.InputDataInteraction("Enter command text"),
			_ => currentCommand.Request = input
		};
	}
}
