using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Entities;
using MissBot.Entities.Query;
using MissBot.Entities.Results;
using MissCore;using MissCore.Data;
using MissCore.Data.Context;
using MissCore.Handlers;
using MissCore.Response;

namespace BotService.Internal{    internal class BotBuilder<TBot> : BotBuilder, IBotBuilder<TBot> where TBot : class, IBot    {
        internal static BotBuilder<TBot> GetInstance(IHostBuilder rootHost)        {
            rootHost.ConfigureServices((h, s)
                => s.AddScoped<IBotServicesProvider, BotServicesProvider>());
            return new BotBuilder<TBot>(rootHost);        }        internal BotBuilder(IHostBuilder rootHost)
            => host = rootHost;

        public IBotBuilder<TBot> UseCommandsRepository<THandler>() where THandler : class, IBotCommandsRepository
        {
            host.ConfigureServices((h, s)
                => s.AddScoped<IBotCommandsRepository, THandler>());
            return this;        }        public IBotBuilder<TBot> UseUpdateHandler<THandler>() where THandler : class, IAsyncUpdateHandler<TBot>        {
            host.ConfigureServices((h, Services)
                => Services.AddScoped<IAsyncUpdateHandler<TBot>, THandler>());
            return this;        }

        public IBotBuilder<TBot> UseCommandDispatcher<THandler>() where THandler : class, IAsyncBotCommandDispatcher        {
            host.ConfigureServices((h, s)
                =>
                {
                    s.AddScoped<IAsyncBotCommandDispatcher, THandler>();
                    s.AddScoped<IResponse<BotCommand>, Response<BotCommand>>();
                });

            _components.Add(
                next =>
                context =>
                    {
                        if (context.Get<Update<TBot>>() is UnitUpdate upd && upd.IsCommand)
                            return context.GetAsyncHandler<IAsyncBotCommandDispatcher>().AsyncDelegate(context.SetNextHandler(next));
                        else
                            return next(context);
                    });            return this;        }

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
                {
                    if (context.Contains(Id<ChosenInlineResult>.Value))
                        return context.GetAsyncHandlerOf<ChosenInlineResult>().AsyncDelegate(context.SetNextHandler(next));
                    else
                        return next(context);
                });
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
                {
                    if (context.Contains(Id<CallbackQuery>.Value))
                        return context.GetAsyncHandlerOf<CallbackQuery>().AsyncDelegate(context.SetNextHandler(next));
                    else
                        return next(context);
                });
            return this;
        }
        public IBotBuilder<TBot> UseInlineHandler<THandler>() where THandler : class, IAsyncHandler<InlineQuery>
        {
            host.ConfigureServices((h, Services)
                =>
                {
                    Services.AddScoped<IAsyncHandler<InlineQuery>, THandler>();                    
                    Services.AddScoped<InlineResponse<InlineQuery>>();
                    Services.AddScoped<IContext<InlineQuery>, Context<InlineQuery>>();
                });
            _components.Add(
                next =>
                context =>
                {
                    if (context.Contains(Id<InlineQuery>.Value))
                        return context.GetAsyncHandlerOf<InlineQuery>().AsyncDelegate(context.SetNextHandler(next));
                    else
                        return next(context);
                });
            return this;
        }

        public IBotBuilder<TBot> Use<THandler>() where THandler : class, IAsyncHandleComponent        {
            host.ConfigureServices((h, Services)
                => Services.AddScoped<THandler>());            _components.Add(                next =>                context =>                     context.GetBotService<THandler>().HandleAsync(context, next));// .GetNextHandler(next));            return this;        }

        public IBotBuilder<TBot> AddRepository<TRepository, TImplementatipon>() where TRepository : class, IRepository where TImplementatipon : class, TRepository
        {
            host.ConfigureServices((h, s)
                => s.AddScoped<TRepository, TImplementatipon>());
            return this;
        }

        public IBotBuilder<TBot> UseMessageHandler<THandler>() where THandler : class, IAsyncHandler<Message>
        {
            host.ConfigureServices((h, Services)
                =>
                {
                    Services.AddScoped<IAsyncHandler<Message>, THandler>();
                    Services.AddScoped<IContext<Message>, Context<Message>>();
                    Services.AddScoped<IResponse<Message>, Response<Message>>();
                });
            _components.Add(                next =>                context =>
                {
                    if (context.Contains(Id<Message>.Value))
                        return context.GetAsyncHandlerOf<Message>().AsyncDelegate(context.SetNextHandler(next));
                    else
                        return next(context);
                });
            return this;
        }       
    }    internal abstract class BotBuilder : IBotBuilder    {
        protected IHostBuilder host;        internal AsyncHandler HandlerDelegate { get; private set; }        protected readonly ICollection<Func<AsyncHandler, AsyncHandler>> _components;        internal BotBuilder()
        {            _components = new List<Func<AsyncHandler, AsyncHandler>>();        }                      IBotBuilder IBotBuilder.AddInputParametersHandler()
        {
            host.ConfigureServices((h, Services)
            =>
            {
                Services.AddScoped<InputParametersHandler>();
            });
            return this;
        }        public IBotBuilder Use(Func<IHandleContext, AsyncHandler> component)        {            throw new NotImplementedException();        }


        public AsyncHandler BuildHandler()
        {
            AsyncHandler handle = context =>
            {
                Update upd = context.Any<Update>();
                context.IsHandled = true;
#if DEBUG
                Console.WriteLine("\t\tNo handler for update {0} of type {1}.", upd.Id, upd.Type);
#endif
                return Task.FromResult(1);
            };

            foreach (var component in _components.Reverse())
                handle = component(handle);

            return HandlerDelegate = handle;
        }

        #region Commands    


        public IBotBuilder AddCommand<TCommand, THandler>() where THandler : BotCommandHandler<TCommand> where TCommand : BotCommand, IBotCommand        {
            host.ConfigureServices((h, Services)
                =>
            {
                Services.AddScoped<IAsyncHandler<TCommand>, THandler>();
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

        public IBotBuilder AddAction<TAction, THandler>() where THandler : class, IAsyncHandler<TAction> where TAction : class, IBotUnitAction
        {
            host.ConfigureServices((h, Services)
            =>
            {
                Services.AddScoped<IAsyncHandler<TAction>, THandler>();
                Services.AddScoped<IResponse<TAction>, Response<TAction>>();
                Services.AddScoped<IContext<TAction>, Context<TAction>>();
            });
            return this;
        }

        IBotBuilder IBotBuilder.AddCustomCommandCreator<TCreator>()
        {
            host.ConfigureServices((h, Services)
            =>
            {
                Services.AddScoped<ICreateBotCommandHandler, TCreator>();                
            });
            return this;
        }
        #endregion
    }}