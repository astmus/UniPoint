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
            this.context = context;
            if (context.Any<UnitUpdate>() is UnitUpdate upd)
            {
                var input = upd.StringContent;
                if (CurrentHandler == null)
                {
                    input = null;
                    Response = context.BotServices.Response<BotCommand>();
                    Initialize();
                    if (currentUnit.ParameterIndex == Handlers.Length)
                    {
                        completedObject = currentUnit;
                        return;
                    }
                    CurrentHandler = Handlers[currentUnit.ParameterIndex];
                }

                switch (CurrentHandler(context, input, currentUnit.CurrentParameterName))
                {
                    case IResponse:                    
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

        FormattableUnitActionBase completedObject;
        public async Task<FormattableUnitActionBase> HandleAsync<TUnitAction>(IBotUnitAction<TUnitAction> action, IHandleContext context, CancellationToken cancel) where TUnitAction : UnitBase
        {            
            currentUnit = context.Get<FormattableUnitAction>(action.Identifier);
            currentUnit ??= FormattableUnitAction.Create(action.Payload, action.GetParameters().ToArray());
            currentUnit["@Id"] = action.Identifier.id;

            currentUnit.SetupParameterPosition();

            await HandleAsync(context);

            if (!context.IsHandled.HasValue)
                context.IsHandled = false;
            return completedObject;
        }

        protected object SetParameter<T>(IHandleContext context, T input, string parameterName) where T:struct
            => currentUnit[parameterName] = input;
        
        protected object SetIntParameter(IHandleContext context, string input, string parameterName)
        {
            int result;
            if (int.TryParse(input, out result))
                return SetParameter<int>(context, result, parameterName);
            else
                return ReTry(SetIntParameter, "Invalid parameter format need number value");
        }
    }
}
