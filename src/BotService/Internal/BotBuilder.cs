using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Abstractions.Response;
using MissBot.Entities;
using MissBot.Entities.Query;
using MissBot.Entities.Results;
using MissCore;using MissCore.Data;
using MissCore.Data.Context;

namespace BotService.Internal{    internal class BotBuilder<TBot> : BotBuilder, IBotBuilder<TBot> where TBot : class, IBot    {
        internal static BotBuilder<TBot> GetInstance(IHostBuilder rootHost)        {
            rootHost.ConfigureServices((h, s)
                => s.AddScoped<IBotServicesProvider, BotServicesProvider>());
            return new BotBuilder<TBot>(rootHost);        }        internal BotBuilder(IHostBuilder rootHost)
            => host = rootHost;

        public IBotBuilder<TBot> UseCommandsRepository<THandler>() where THandler : class, IRepository<BotCommand>
        {
            host.ConfigureServices((h, s)
                => s.AddScoped<IRepository<BotCommand>, THandler>());
            return this;        }        public IBotBuilder<TBot> UseUpdateHandler<THandler>() where THandler : class, IAsyncUpdateHandler<TBot>        {
            host.ConfigureServices((h, Services)
                => Services.AddScoped<IAsyncUpdateHandler<TBot>, THandler>());
            return this;        }

        public IBotBuilder<TBot> UseCommandDispatcher<THandler>() where THandler : class, IAsyncBotCommandDispatcher        {
            host.ConfigureServices((h, s)
                => s.AddScoped<IAsyncBotCommandDispatcher, THandler>());
            _components.Add(
                next =>
                context =>
                     context.GetNextHandler<IAsyncBotCommandDispatcher>().ExecuteAsync(context.SetNextHandler(context, next)));            return this;        }

        public IBotBuilder<TBot> UseInlineAnswerHandler<THandler>() where THandler : class, IAsyncHandler<ChosenInlineResult>
        {
            host.ConfigureServices((h, Services)
                =>
            {
                Services.AddScoped<IAsyncHandler<ChosenInlineResult>, THandler>();
                Services.AddScoped<IResponse<ChosenInlineResult>, Response<ChosenInlineResult>>();
                Services.AddScoped<IContext<ChosenInlineResult>, Context<ChosenInlineResult>>();
            });
            _components.Add(
                next =>
                context =>
                     context.GetNextHandler<IAsyncHandler<ChosenInlineResult>>().AsyncHandler(context.SetNextHandler(context, next)));
            return this;
        }
        public IBotBuilder<TBot> UseCallbackDispatcher<THandler>() where THandler : class, IAsyncHandler<CallbackQuery>
        {
            host.ConfigureServices((h, Services)
                =>
            {
                Services.AddScoped<IAsyncHandler<CallbackQuery>, THandler>();
                Services.AddScoped<IResponseNotification, CallBackNotification>();
                Services.AddScoped<IResponse<CallbackQuery>, Response<CallbackQuery>>();
                Services.AddScoped<IContext<CallbackQuery>, Context<CallbackQuery>>();
            });
            _components.Add(
                next =>
                context =>
                     context.GetNextHandler<IAsyncHandler<CallbackQuery>>().AsyncHandler(context.SetNextHandler(context, next)));
            return this;
        }
        public IBotBuilder<TBot> UseInlineHandler<THandler>() where THandler : class, IAsyncHandler<InlineQuery>
        {
            host.ConfigureServices((h, Services)
            =>
            {
                Services.AddScoped<IAsyncHandler<InlineQuery>, THandler>();
                Services.AddScoped<IResponse<InlineQuery>, InlineResponse<InlineQuery>>();
                Services.AddScoped<IContext<InlineQuery>, Context<InlineQuery>>();
            });
            _components.Add(
                next =>
                context =>
                     context.GetNextHandler<IAsyncHandler<InlineQuery>>().AsyncHandler(context.SetNextHandler(context, next)));
            return this;
        }

        public IBotBuilder<TBot> Use<THandler>() where THandler : class, IAsyncHandler        {
            host.ConfigureServices((h, Services)
                => Services.AddScoped<THandler>());            _components.Add(                next =>                context =>                     context.Handler(context.SetNextHandler(context, next)));            return this;        }

        public IBotBuilder<TBot> AddRepository<TRepository, TImplementatipon>() where TRepository : class, IRepository where TImplementatipon : class, TRepository
        {

            host.ConfigureServices((h, s)
                => s.AddScoped<TRepository, TImplementatipon>());
            return this;
        }
    }    internal abstract class BotBuilder : IBotBuilder    {
        protected IHostBuilder host;        internal AsyncHandler HandlerDelegate { get; private set; }        protected readonly ICollection<Func<AsyncHandler, AsyncHandler>> _components;        internal BotBuilder()
        {            _components = new List<Func<AsyncHandler, AsyncHandler>>();        }        public IBotBuilder Use(Func<AsyncHandler, AsyncHandler> middleware)        {            throw new NotImplementedException();        }        public IBotBuilder AddActionHandler<THandler>() where THandler : class, IAsyncHandler        {
            host.ConfigureServices((h, Services)
                => Services.AddScoped<THandler>());            _components.Add(                next =>                context =>                     context.Handler(context.SetNextHandler(context, next)));            return this;        }        public IBotBuilder Use<THandler>(THandler handler) where THandler : class, IAsyncHandler        {
            host.ConfigureServices((h, Services)
              => Services.AddScoped<THandler>());            _components.Add(next =>                context                    => handler.AsyncHandler(context.SetNextHandler(context, next)));            return this;        }        public IBotBuilder Use(Func<IHandleContext, AsyncHandler> component)        {            throw new NotImplementedException();        }


        public AsyncHandler BuildHandler()
        {
            AsyncHandler handle = context =>
            {
                Update upd = context.Any<Update>();
                //if (context.IsHandled != true && context.IsHandled != false)
                context.IsHandled = true;                                                                          
#if DEBUG
                Console.WriteLine("No handler for update {0} of type {1}.", upd.Id, upd.Type);
#endif
                return Task.FromResult(1);
            };

            foreach (var component in _components.Reverse())
                handle = component(handle);

            return HandlerDelegate = handle;
        }

        #region Commands    
        public IBotBuilder AddCommand<TCommand, THandler, TResponse>() where
              THandler : BotCommandHandler<TCommand> where
              TCommand : BotCommand, IBotCommand where
              TResponse : class, IResponse<TCommand>
        {
            host.ConfigureServices((h, Services)
                =>
            {
                Services.AddScoped<IAsyncHandler<TCommand>, THandler>();
                Services.AddScoped<TCommand>();
                Services.AddScoped<IResponse<TCommand>, TResponse>();
                Services.AddScoped<IContext<TCommand>, Context<TCommand>>();
            });
            return this;
        }

        public IBotBuilder AddCommand<TCommand, THandler>() where THandler : BotCommandHandler<TCommand> where TCommand : BotCommand, MissBot.Abstractions.IBotCommand        {
            host.ConfigureServices((h, Services)
                =>
            {
                Services.AddScoped<IAsyncHandler<TCommand>, THandler>();
                Services.AddScoped<TCommand>();
                Services.AddScoped<IResponse<TCommand>, Response<TCommand>>();
                Services.AddScoped<IContext<TCommand>, Context<TCommand>>();
            });
            //_components.Add(
            //    next =>
            //    context =>
            //         context.BotServices.
            //            GetRequiredService<IAsyncBotCommandHandler>().HandleAsync<TCommand>(context));
            return this;        }

        public void Build()
        {
            if (HandlerDelegate is null)
                BuildHandler();
        }

        public IBotBuilder AddAction<TAction, THandler>() where THandler : class, IAsyncHandler<TAction> where TAction : class, IBotAction
        {
            host.ConfigureServices((h, Services)
            =>
            {
                Services.AddScoped<IAsyncHandler<TAction>, THandler>();
                Services.AddScoped<TAction>();
                Services.AddScoped<IResponse<TAction>, Response<TAction>>();
                Services.AddScoped<IContext<TAction>, Context<TAction>>();
            });
            return this;
        }






        #endregion                                                              }}