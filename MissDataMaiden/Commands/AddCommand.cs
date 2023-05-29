using BotService;
using MissBot.Abstractions;
using MissBot.Entities;
using MissCore.Data;

namespace MissDataMaiden
{
    public class AddCommandHadler : CreateBotCommandHandler<CustomCommand>
    {
        async Task Save(IHandleContext context)
        {
            var listCmds = context.Bot.Commands.ToList();
            listCmds.Add(cmd);
            var chat = context.Get<Chat>() ?? context.Any<UnitUpdate>().Chat;
            var success = await context.BotServices.Client.SyncCommandsAsync(listCmds, Telegram.Bot.Types.BotCommandScope.Chat(chat.Id));
            if (success)
            {
                context.Bot.Commands.Add(cmd);
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
        object OfferCompleteOptions(IHandleContext context, string input, string parameterName) => input switch
        {
            null => Response.InputData("Save command?", ChatActions.Create(1, nameof(Save), nameof(Cancel))),
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
