using System;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Entities;
using MissCore.Bot;

namespace MissCore.Handlers
{


    public class BotUnitActionHandler : BotUnitActionHadlerBase<BotUnitAction>
    {
        private readonly IRepository<BotCommand> repository;
        


        protected override void Initialize()
        {
            var args = Enumerable.Repeat<AsyncInputHandler>(SetAliase, currentUnit.ArgumentCount).ToArray();
            JoinHandlers(args);
        }

        object OfferCompleteOptions(IHandleContext context, string input) => input switch
        {
            null => Response.InputData("Save command?", ChatActions.Create(nameof(Save), nameof(Cancel))),
            nameof(Save) => Save(context),
            nameof(Cancel) => Cancel(context),
            _ => null
        };

        async Task Save(IHandleContext context)
        {
            var listCmds = context.Bot.Commands.ToList();
            //listCmds.Add(add);
            var success = await context.BotServices.Client.SyncCommandsAsync(listCmds, Telegram.Bot.Types.BotCommandScope.Chat(context.Get<Chat>().Id));
            if (success)
            {
             //   context.Bot.Commands.Add(add);
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
                Response.InputData($"Enter parameter: '{parameterName}'"),
            var value when value.IndexOf(' ') < 0 =>
                currentUnit[parameterName] = value,
                //add.Aliase = value,
            var value when value.IndexOf(' ') > -1 =>
                ReTry(SetAliase, "Invalid aliase format"),
            _ => null
        };
        object SetDescription(IHandleContext context, string input) => input switch
        {
            null => Response.InputData("Enter command description"),
            _ =>/* add.Description =*/ input
        };
        object SetCommand(IHandleContext context, string input) => input switch
        {
            null => Response.InputData("Enter command text"),
            _ => /*add.Request =*/ input
        };
    }
}
