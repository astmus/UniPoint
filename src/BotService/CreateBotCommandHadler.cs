using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissBot.Abstractions.Utils;
using MissCore.Data;

namespace BotService
{
    public class CreateBotCommandHandler<TCommand> : ICreateBotCommandHandler<TCommand> where TCommand : BotCommand
    {
        protected TCommand cmd;

        AsyncInputHandler CurrentHandler;
        List<AsyncInputHandler> Handlers;
        Position position;
        protected IResponse<BotCommand> Response;
        IHandleContext context;
        async Task HandleInputAsync(IHandleContext context)
        {
            this.context = context;
            if (context.Any<UnitUpdate>() is UnitUpdate upd)
            {
                var input = upd.StringContent;
                if (CurrentHandler == null)
                {
                    input = null;
                    Handlers = new List<AsyncInputHandler>();
                    Response = context.BotServices.Response<BotCommand>();
                    InitHandlers();
                    CurrentHandler = Handlers[position.Current];
                }

                switch (CurrentHandler(context, input, string.Empty))
                {
                    case IResponse:
                    case AsyncInputHandler handler:
                        break;
                    case Task task:
                        await task.ConfigureAwait(false);
                        break;
                    default:
                        MoveNext();
                        CurrentHandler(context, null, string.Empty);
                        break;
                }
                await Response.Commit();
            }
        }
        protected virtual void InitHandlers()
        {
            AddHandlers(SetAliase, SetDescription, SetCommand);
        }
        void MoveNext()
        {
            if (position.Forward() < Handlers.Count)
                CurrentHandler = Handlers[position.Current];
            else context.IsHandled = true;
        }

        void MoveBack()
            => CurrentHandler = Handlers[position.Back()];

        protected void AddHandlers(params AsyncInputHandler[] handlers)
            => Handlers.AddRange(handlers);

        protected AsyncInputHandler ReTry(AsyncInputHandler handler, string message)
        {
            handler(null, null, string.Empty);
            //Response.Content = message + "\n" + Response.Content;
            return CurrentHandler = handler;
        }

        protected object SetAliase(IHandleContext context, string input, string parameterName) => input switch
        {
            null =>
                Response.InputData("Enter unique command alise"),
            var value when value.IndexOf(' ') < 0 =>
                cmd.Action = value,
            var value when value.IndexOf(' ') > -1 =>
                ReTry(SetAliase, "Invalid aliase format"),
            _ => null
        };
        protected object SetDescription(IHandleContext context, string input, string parameterName) => input switch
        {
            null => Response.InputData("Enter command description"),
            { Length: 0 } => ReTry(SetDescription, "Enter command description"),
            _ => cmd.Description = input
        };
        protected object SetCommand(IHandleContext context, string input, string parameterName) => input switch
        {
            null => Response.InputData("Enter command text"),
            _ => cmd.Payload = input
        };

        public Task CreateAsync(IHandleContext context, CancellationToken cancel = default)
        {
            cmd = context.BotServices.Activate<TCommand>();
            context.SetNextHandler(HandleInputAsync);
            return HandleInputAsync(context);
        }
    }
}
