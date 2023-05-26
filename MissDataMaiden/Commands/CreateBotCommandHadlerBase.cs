using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissBot.Abstractions.Utils;
using MissCore.Data;

namespace MissDataMaiden
{
    public abstract class CreateBotCommandHadlerBase : IAsyncHandler<BotCommand>
    {
        AsyncInputHandler CurrentHandler;
        AsyncInputHandler[] Handlers;
        Position position;
        protected IResponse<BotCommand> Response;
        public AsyncHandler AsyncDelegate { get; protected set; }
        public CreateBotCommandHadlerBase()
        {
            AsyncDelegate = HandleAsync;
        }
        public Task HandleAsync(BotCommand data, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }
        async Task HandleAsync(IHandleContext context)
        {
            if (context.Any<UnitUpdate>() is UnitUpdate upd)
            {
                var input = upd.StringContent;
                if (CurrentHandler == null)
                {                    
                    input = null;
                    Response = context.BotServices.Response<BotCommand>();
                    Initialize();
                    CurrentHandler = Handlers[position.Current];
                }

                switch (CurrentHandler(context, input))
                {
                    case IResponse:
                        await Response.Commit(); return;
                    case AsyncInputHandler handler:
                        await Response.Commit(); return;
                    case Task task:
                        await task; break;
                    default:
                        MoveNext();
                        CurrentHandler(context, null); break;
                }
                await Response.Commit();
            }
        }

        protected void JoinHandlers(params AsyncInputHandler[] handlers)
            => Handlers = handlers;
        protected abstract void Initialize();
        void MoveBack()
            => CurrentHandler = Handlers[position.Bask];

        void MoveNext()
            => CurrentHandler = Handlers[position.Forward];

        protected AsyncInputHandler ReTry(AsyncInputHandler handler, string message)
        {
            handler(null, null);
            Response.Content = message + "\n" + Response.Content;
            return CurrentHandler = handler;
        }
    }
}
