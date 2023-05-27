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
    
        protected IResponse<BotCommand> Response;
        protected IHandleContext context;
        protected FormattableUnitAction currentUnit { get; private set; }
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
                    CurrentHandler = Handlers[currentUnit.ParameterIndex];
                }

                switch (CurrentHandler(context, input, currentUnit.CurrentParameterName))
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
                            CurrentHandler(context, null, currentUnit.CurrentParameterName); break;
                }
                await Response.Commit();
            }
        }

        protected void JoinHandlers(params AsyncInputHandler[] handlers)
            => Handlers = handlers;
        protected abstract void Initialize();
        void MoveBack()
            => CurrentHandler = Handlers[currentUnit.BackParameter];

        bool MoveNext()
        {
            CurrentHandler = Handlers[currentUnit.ParameterIndex];
            return currentUnit.ForwardParameter < Handlers.Length;
        }

        protected AsyncInputHandler ReTry(AsyncInputHandler handler, string message)
        {
            handler(null, null, currentUnit.CurrentParameterName);
            Response.Content = message + "\n" + Response.Content;
            return CurrentHandler = handler;
        }

        FormattableString completedObject;
        public async Task<FormattableString> HandleAsync<TUnitAction>(IBotUnitAction<TUnitAction> action, IHandleContext context, CancellationToken cancel) where TUnitAction : UnitBase
        {            
            currentUnit = context.Get(FormattableUnitAction.Create(action.Payload, action.GetParameters().ToArray()), action.Identifier);
            currentUnit["@Id"] = action.Identifier.id;
            currentUnit.InitParameterPosition();
            this.context = context;
            await HandleAsync(context);
            if (!context.IsHandled.HasValue)
                context.IsHandled = false;
            return completedObject;
        }
    }
}
