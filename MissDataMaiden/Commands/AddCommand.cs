using BotService;
using MissBot.Abstractions;
using MissBot.Entities;
using MissBot.Entities.API;
using MissCore.Data;

namespace MissDataMaiden.Commands
{
    public class AddCommandHadler : CreateBotCommandHandler<CustomCommand>
    {
        async Task Save(IHandleContext context)
        {
            var listCmds = context.Bot.Commands.ToList();
            listCmds.Add(cmd);
            var chat = context.Get<Chat>() ?? context.Any<UnitUpdate>().Chat;
            var success = await context.BotServices.Client.SyncCommandsAsync(listCmds, BotCommandScope.Chat(chat.Id));
            if (success)
            {
                context.Bot.Commands.Add(cmd);
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
        object OfferCompleteOptions(IHandleContext context, string input, string parameterName) => input switch
        {
            null => Response.InputDataInteraction("Save command?", ChatActions.Create(1, nameof(Save), nameof(Cancel))),
            nameof(Save) => Save(context),
            nameof(Cancel) => Cancel(context),
            _ => null
        };

        protected override void InitHandlers()
        {
            base.InitHandlers();
            AddHandlers(OfferCompleteOptions);
        }
    }
}
