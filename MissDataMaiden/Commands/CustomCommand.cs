using System.Diagnostics.CodeAnalysis;
using Azure;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Components.Web;
using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Abstractions.Utils;
using MissBot.Entities;
using MissCore.Bot;
using MissCore.Data;
using MissCore.Data.Entities;
using MissCore.Handlers;

namespace MissDataMaiden
{
    public record CustomCommand : BotUnitCommand
    {
        [NotNull]
        public string Aliase { get => Action; set => Action = value; }
        public string Request { get => Payload; set => Payload = value; }
    }

    public class CustomCommandCreateHandler : BotUnitActionHadlerBase<CustomCommand>
    {
        private readonly IRepository<BotCommand> repository;        

        CustomCommand currentCommand;
        protected override void Initialize()
        {
            currentCommand = new CustomCommand();
            JoinHandlers(SetAliase, SetDescription, SetCommand, OfferCompleteOptions);
        }

        object OfferCompleteOptions(IHandleContext context, string input, string parameterName) => input switch
        {
            null => Response.InputData("Save command?", ChatActions.Create(nameof(Save), nameof(Cancel))),
            nameof(Save) => Save(context),
            nameof(Cancel) => Cancel(context),
            _ => null
        };

        async Task Save(IHandleContext context)
        {
            var listCmds = context.Bot.Commands.ToList();
            listCmds.Add(currentCommand);
            var success = await context.BotServices.Client.SyncCommandsAsync(listCmds, Telegram.Bot.Types.BotCommandScope.Chat(context.Get<Chat>().Id));
            if (success)
            {
                context.Bot.Commands.Add(currentCommand);
                Response.CompleteInput("Command added");
            }
            else
                Response.CompleteInput("Save failed");

            context.IsHandled = true;
        }

        Task Cancel(IHandleContext context)
        {
            Response.CompleteInput("Operation cancelled");
            context.IsHandled = true;
            return Task.CompletedTask;
        }

        object SetAliase(IHandleContext context, string input, string parameterName) => input switch
        {
            null =>
                Response.InputData("Enter unique command alise"),
            var value when value.IndexOf(' ') < 0 =>
                currentCommand.Aliase = value,
            var value when value.IndexOf(' ') > -1 =>
                ReTry(SetAliase, "Invalid aliase format"),
            _ => null
        };
        object SetDescription(IHandleContext context, string input, string parameterName) => input switch
        {
            null => Response.InputData("Enter command description"),
            _ => currentCommand.Description = input
        };
        object SetCommand(IHandleContext context, string input, string parameterName) => input switch
        {
            null => Response.InputData("Enter command text"),            
            _ => currentCommand.Request = input
        };
    }
}
