using Microsoft.Extensions.DependencyInjection.Extensions;
using MissBot.Abstractions;
using MissBot.Common;
using MissBot.Response;
using MissCore;using MissCore.Configuration;using MissCore.Data.Context;
using MissCore.Entities;using Telegram.Bot.Types;

namespace BotService.Internal{    internal class BotBuilder<TBot> : BotBuilder, IBotBuilder<TBot> where TBot : class, IBot    {        internal static BotBuilder<TBot> instance;        internal static BotBuilder<TBot> Instance { get => instance; }        internal override IServiceCollection Services { get; set; }        public IServiceCollection BotServices            => Services;        static IHostBuilder host;        internal static BotBuilder<TBot> GetInstance(IHostBuilder rootHost)        {            host = rootHost;            host.ConfigureServices((h, s) => { instance.Services.AddTransient(sp => h.Configuration); });            return Instance;        }        static BotBuilder()        {            instance = new BotBuilder<TBot>();            instance.Services = new ServiceCollection();
        }        internal BotBuilder()
        {            Services = Instance?.Services;        }        public IBotBuilder<TBot> UseCommndFromAttributes()        {            var cmds = typeof(TBot).GetCommandsFromAttributes();            _botCommands.AddRange(cmds);            _botCommands.ForEach(c => Services.AddScoped(c.CmdType));            return this;        }        public IBotServicesProvider BotServicesProvider()            => new BotServicesProvider(Instance.Services.BuildServiceProvider());        public IBotBuilder<TBot> UseUpdateHandler<THandler>() where THandler : class, IAsyncUpdateHandler<TBot>        {            Services.AddTransient<IAsyncUpdateHandler<TBot>, THandler>();
            Services.TryAddScoped<IBotBuilder<TBot>>(sp => BotBuilder<TBot>.Instance);
            host.ConfigureServices((h, s) => s.AddTransient<IAsyncUpdateHandler<TBot>, THandler>());            return this;        }

        public IBotBuilder<TBot> UseCommandDispatcher<THandler>() where THandler : class, IAsyncBotCommandHandler        {            Services.AddScoped<IAsyncBotCommandHandler, THandler>();
            Services.TryAddScoped<IBotBuilder<TBot>>(sp => BotBuilder<TBot>.Instance);
            host.ConfigureServices((h, s)
                => s.AddScoped<IAsyncBotCommandHandler, THandler>());
            _components.Add(
                next =>
                context =>
                     context.NextHandler<IAsyncBotCommandHandler>().ExecuteAsync(context.SetupData(context, next)));            return this;        }

        public IBotBuilder<TBot> UseCallbackDispatcher<THandler>() where THandler : class, IAsyncHandler<CallbackQuery>
        {
            Services.AddScoped<IAsyncHandler<CallbackQuery>, THandler>();
            Services.AddScoped<IResponse, Response>();
            Services.AddScoped<IResponse<CallbackQuery>, Response<CallbackQuery>>();
            Services.AddScoped<IContext<CallbackQuery>, Context<CallbackQuery>>();
            _components.Add(
                next =>
                context =>
                     context.NextHandler<IAsyncHandler<CallbackQuery>>().AsyncHandler(context.SetupData(context, next)));
            return this;
        }
        public IBotBuilder<TBot> UseInlineHandler<THandler>() where THandler : class, IAsyncHandler<InlineQuery>
        {
            Services.AddScoped<IAsyncHandler<InlineQuery>, THandler>();            
            Services.AddScoped<IResponse, Response>();
            Services.AddScoped<IResponse<InlineQuery>, InlineResponse<InlineQuery>>();
            Services.AddScoped<IContext<InlineQuery>, Context<InlineQuery>>();
            _components.Add(
                next =>
                context =>
                     context.NextHandler<IAsyncHandler<InlineQuery>>().AsyncHandler(context.SetupData(context, next)));
            return this;
        }        public IBotBuilder<TBot> UseContextHandler<THandler>() where THandler : class, IContextHandler<TBot>        {            Services.AddSingleton<THandler>();
            Services.TryAddScoped<IBotBuilder<TBot>>(sp => BotBuilder<TBot>.Instance);            host.ConfigureServices((h, s) => s.AddSingleton<IContextHandler<TBot>, THandler>());
            //_components.Add(            //    next =>            //    context =>
            //        context.NextHandler<THandler>().AsyncHandler(context.SetupData(context, next)));            return this;        }
        public IBotBuilder<TBot> Use<THandler>() where THandler : class, IAsyncHandler        {            Services.AddScoped<THandler>();            _components.Add(                next =>                context =>                     context.NextHandler<THandler>().AsyncHandler(context.SetupData(context, next)));            return this;        }


    }    internal abstract class BotBuilder : IBotBuilder    {        internal AsyncHandler HandlerDelegate { get; private set; }        internal abstract IServiceCollection Services { get; set; }        protected readonly ICollection<Func<AsyncHandler, AsyncHandler>> _components;        protected List<IBotCommandInfo> _botCommands = new List<IBotCommandInfo>();        internal BotBuilder()        {            _components = new List<Func<AsyncHandler, AsyncHandler>>();        }        public IBotBuilder Use(Func<AsyncHandler, AsyncHandler> middleware)        {            throw new NotImplementedException();        }        public IBotBuilder AddActionHandler<THandler>() where THandler : class, IAsyncHandler        {            Services.AddScoped<THandler>();            _components.Add(                next =>                context =>                     context.NextHandler<THandler>().AsyncHandler(context.SetupData(context, next)));            return this;        }        public IBotBuilder Use<THandler>(THandler handler) where THandler : class, IAsyncHandler        {            Services.AddScoped<THandler>();            _components.Add(next =>                context                    => handler.AsyncHandler(context.SetupData(context, next)));            return this;        }        public IBotBuilder Use(Func<IHandleContext, AsyncHandler> component)        {            throw new NotImplementedException();        }
        public AsyncHandler BuildHandler()        {
            AsyncHandler handle = context =>
                {
                    Update upd = context.Any<Update>();
                    upd.IsHandled = true;
#if DEBUG                    Console.WriteLine("No handler for update {0} of type {1}.", upd.UpdateId, upd.UpdateId);
#endif                    return Task.FromResult(1);
                };            foreach (var component in _components.Reverse())                handle = component(handle);            return HandlerDelegate = handle;        }


        #region Commands    
        public IBotBuilder AddCommand<TCommand, THandler, TResponse>() where
              THandler : BotCommandHandler<TCommand> where
              TCommand : class, IBotCommand where
              TResponse :class, IResponse<TCommand>
        {
            Services.AddScoped<IAsyncHandler<TCommand>, THandler>();            Services.AddScoped<TCommand>();
            Services.AddScoped<IResponse, Response>();
            Services.AddScoped<IResponse<TCommand>, TResponse>();
            Services.AddScoped<IContext<TCommand>, Context<TCommand>>();
            return this;
        }

        public IBotBuilder AddCommand<TCommand, THandler>() where THandler : BotCommandHandler<TCommand> where TCommand : class, IBotCommand        {                        Services.AddScoped<IAsyncHandler<TCommand>, THandler>();            Services.AddScoped<TCommand>();
            Services.AddScoped<IResponse, Response>();
            Services.AddScoped<IResponse<TCommand>, Response<TCommand>>();
            Services.AddScoped<IContext<TCommand>, Context<TCommand>>();
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

        public IBotBuilder AddAction<TAction, THandler>() where THandler : class, IAsyncHandler<TAction> where TAction:class,IEntityAction
        {
            Services.AddScoped<IAsyncHandler<TAction>, THandler>();            Services.AddScoped<TAction>();
            Services.AddScoped<IResponse, Response>();
            Services.AddScoped<IResponse<TAction>, Response<TAction>>();
            Services.AddScoped<IContext<TAction>, Context<TAction>>();
            return this;
        }

        #endregion     }}