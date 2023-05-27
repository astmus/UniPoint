using System.Diagnostics.CodeAnalysis;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Abstractions.Utils;
using MissBot.Entities;
using MissCore.Bot;
using MissCore.Data;

namespace MissDataMaiden
{
   

    public class AddCommandHadler : IAsyncHandler<AddCommandHadler>
    {
        private readonly IRepository<BotCommand> repository;
        public AddCommandHadler()
            => AsyncDelegate = HandleAsync;
        CustomCommand add;
        public AsyncHandler AsyncDelegate { get; protected set; }
        AsyncInputHandler CurrentHandler;
        AsyncInputHandler[] Handlers;
        Position position;
        IResponse<BotCommand> Response;
        async Task HandleAsync(IHandleContext context)
        {
            if (context.Any<UnitUpdate>() is UnitUpdate upd)
            {
                var input = upd.StringContent;
                if (CurrentHandler == null)
                {
                    add ??= new CustomCommand();
                    input = null;
                    Response = context.BotServices.Response<BotCommand>();
                    JoinHandlers(SetAliase,SetDescription, SetCommand, OfferCompleteOptions);
                    CurrentHandler = Handlers[position.Current];
                }

                switch (CurrentHandler(context, input, string.Empty))
                {
                    case IResponse:
                        await Response.Commit(); return;
                    case AsyncInputHandler handler:
                        await Response.Commit(); return;
                    case Task task:
                        await task; break;
                    default:
                        MoveNext();
                        CurrentHandler(context, null, string.Empty); break;
                }
                await Response.Commit();
            }
        }

        void MoveNext()
            => CurrentHandler = Handlers[position.Forward];

        void MoveBack()
            => CurrentHandler = Handlers[position.Back];

        void JoinHandlers(params AsyncInputHandler[] handlers)
            => Handlers = handlers;

        AsyncInputHandler ReTry(AsyncInputHandler handler, string message)
        {
            handler(null, null, string.Empty);
            Response.Content = message + "\n" + Response.Content;
            return CurrentHandler = handler;
        }

        public Task HandleAsync(AddCommandHadler data, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
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
            listCmds.Add(add);
            var chat = context.Get<Chat>() ?? context.Any<UnitUpdate>().Chat;
            var success = await context.BotServices.Client.SyncCommandsAsync(listCmds, Telegram.Bot.Types.BotCommandScope.Chat(chat.Id));
            if (success)
            {
                context.Bot.Commands.Add(add);
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
                add.Aliase = value,
            var value when value.IndexOf(' ') > -1 =>
                ReTry(SetAliase, "Invalid aliase format"),
            _ => null
        };
        object SetDescription(IHandleContext context, string input, string parameterName) => input switch
        {
            null => Response.InputData("Enter command description"),
            _ => add.Description = input
        };
        object SetCommand(IHandleContext context, string input, string parameterName) => input switch
        {
            null => Response.InputData("Enter command text"),            
            _ => add.Request = input
        };
    }
}
