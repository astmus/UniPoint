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
        protected FormattableUnit currentUnit { get; private set; }
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
                    currentUnit.SetupParameterPosition();
                    Response = context.BotServices.Response<BotCommand>();
                    Initialize(currentUnit.ParameterNames);
                    if (currentUnit.ParameterIndex == Handlers.Length)
                    {
                        await complete(currentUnit, context).ConfigureAwait(false);
                        return;
                    }
                    CurrentHandler = Handlers[currentUnit.ParameterIndex];
                }

                switch (CurrentHandler(context, input, currentUnit.CurrentParameterName))
                {
                    case IResponse:                    
                    case AsyncInputHandler handler:
                        break;
                    case Task task:
                        await task.ConfigureAwait(false); break;
                    default:
                        if (MoveNext() is AsyncInputHandler inputHandler)
                            inputHandler(context, null, currentUnit.CurrentParameterName);
                        else
                            await complete(currentUnit, context).ConfigureAwait(false);
                            break;
                }
                await Response.Commit();
            }
        }

        protected void JoinHandlers(params AsyncInputHandler[] handlers)
            => Handlers = handlers;
        protected abstract void Initialize(IEnumerable<string> parameterNames);
        void MoveBack()
            => CurrentHandler = Handlers[currentUnit.Parameter.Back()];

        protected AsyncInputHandler MoveNext()
        {
            while(currentUnit.Parameter.Forward() < Handlers.Length)
                if (currentUnit[currentUnit.CurrentParameterName] is null)
                    return CurrentHandler = Handlers[currentUnit.ParameterIndex];
            return null;
        }

        protected AsyncInputHandler ReTry(AsyncInputHandler handler, string message)
        {
            handler(null, null, currentUnit.CurrentParameterName);
            Response.Content = message + "\n" + Response.Content;
            return CurrentHandler = handler;
        }

        Func<FormattableUnitBase, IHandleContext, Task> complete;
        public async Task HandleAsync<TUnitAction>(Func<FormattableUnitBase, IHandleContext, Task> callBack, IBotUnitAction<TUnitAction> action, IHandleContext context, CancellationToken cancel) where TUnitAction : UnitBase
        {
            complete = callBack;
            currentUnit = context.Get<FormattableUnit>(action.Identifier);
            currentUnit ??= FormattableUnit.Create(action.Payload, action.GetParameters().ToArray());
            currentUnit["Id"] = action.Identifier.id;            

            await HandleAsync(context);

            if (!context.IsHandled.HasValue)
                context.IsHandled = false;
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
