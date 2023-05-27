using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Abstractions.Utils;
using MissCore.Data;

namespace MissCore.Handlers
{
    public abstract class BotUnitActionHadlerBase<TUnit> : IAsyncBotUnitActionHandler where TUnit : UnitBase
    {
        AsyncInputHandler CurrentHandler;
        AsyncInputHandler[] Handlers;
        Position position;
        protected IResponse<BotCommand> Response;
        protected IHandleContext context;
        protected FormattableBotUnit currentUnit { get; private set; }
        public AsyncHandler AsyncDelegate { get; protected set; }
        public BotUnitActionHadlerBase()
        {
            AsyncDelegate = HandleAsync;
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

                switch (CurrentHandler(context, input, currentUnit[position.Current]))
                {
                    case IResponse:
                        await Response.Commit(); return;
                    case AsyncInputHandler handler:
                        await Response.Commit(); return;
                    case Task task:
                        await task; break;
                    default:
                        if (MoveNext() == false)
                            completedObject = currentUnit;
                        else
                            CurrentHandler(context, null, currentUnit[position.Current]); break;
                }
                await Response.Commit();
            }
        }

        protected void JoinHandlers(params AsyncInputHandler[] handlers)
            => Handlers = handlers;
        protected abstract void Initialize();
        void MoveBack()
            => CurrentHandler = Handlers[position.Back];

        bool MoveNext()
        {
            CurrentHandler = Handlers[position.Current];
            return position.Forward < Handlers.Length;
        }

        protected AsyncInputHandler ReTry(AsyncInputHandler handler, string message)
        {
            handler(null, null, currentUnit[position.Current]);
            Response.Content = message + "\n" + Response.Content;
            return CurrentHandler = handler;
        }
        FormattableString completedObject;
        public async Task<FormattableString> HandleAsync<TUnitAction>(IBotUnitAction<TUnitAction> action, IHandleContext context, CancellationToken cancel) where TUnitAction : UnitBase
        {            
            currentUnit = context.Get(FormattableBotUnit.Create(action.Payload, action.GetParameters().ToArray()), action.Identifier);
            this.context = context;
            await HandleAsync(context);
            if (!context.IsHandled.HasValue)
                context.IsHandled = false;
            return completedObject;
        }
    }
}
