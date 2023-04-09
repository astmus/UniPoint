using Microsoft.Extensions.DependencyInjection.Extensions;
using MissBot.Abstractions;
using MissBot.Common;
using MissBot.Response;
using MissCore;using MissCore.Configuration;using MissCore.Data.Context;
using MissCore.Entities;namespace BotService.Internal{    internal class BotBuilder<TBot> : BotBuilder, IBotBuilder<TBot> where TBot : class, IBot    {        internal static BotBuilder<TBot> instance;        internal static BotBuilder<TBot> Instance { get => instance; }        internal override IServiceCollection Services { get; set; }        public IServiceCollection BotServices            => Services;        static IHostBuilder host;        internal static BotBuilder<TBot> GetInstance(IHostBuilder rootHost)        {            host = rootHost;            host.ConfigureServices((h, s) => { instance.Services.AddTransient(sp => h.Configuration); });            return Instance;        }        static BotBuilder()        {            instance = new BotBuilder<TBot>();            instance.Services = new ServiceCollection();
        }        internal BotBuilder()
        {            Services = Instance?.Services;        }        public IBotBuilder<TBot> UseCommndFromAttributes()        {            var cmds = typeof(TBot).GetCommandsFromAttributes();            _botCommands.AddRange(cmds);            _botCommands.ForEach(c => Services.AddScoped(c.CmdType));            return this;        }        IBotServicesProvider sp;        public IBotServicesProvider BotServicesProvider()            => sp ??= new BotServicesProvider(Instance.Services.BuildServiceProvider());        public IBotBuilder<TBot> UseUpdateHandler<THandler>() where THandler : class, IAsyncUpdateHandler<TBot>        {            Services.AddTransient<IAsyncUpdateHandler<TBot>, THandler>();
            Services.TryAddScoped<IBotBuilder<TBot>>(sp => BotBuilder<TBot>.Instance);
            host.ConfigureServices((h, s) => s.AddTransient<IAsyncUpdateHandler<TBot>, THandler>());            return this;        }

        public IBotBuilder<TBot> UseCommandHandler<THandler>() where THandler : class, IAsyncBotCommandHandler        {            Services.AddScoped<IAsyncBotCommandHandler, THandler>();
            Services.TryAddScoped<IBotBuilder<TBot>>(sp => BotBuilder<TBot>.Instance);
            host.ConfigureServices((h, s)
                => s.AddScoped<IAsyncBotCommandHandler, THandler>());
            _components.Add(
                next =>
                context =>
                     context.NextHandler<IAsyncBotCommandHandler>().ExecuteAsync(context.SetupData(context, next)));            return this;        }        public IBotBuilder<TBot> UseContextHandler<THandler>() where THandler : class, IContextHandler<TBot>        {            Services.AddSingleton<THandler>();
            Services.TryAddScoped<IBotBuilder<TBot>>(sp => BotBuilder<TBot>.Instance);            host.ConfigureServices((h, s) => s.AddSingleton<IContextHandler<TBot>, THandler>());
            //_components.Add(            //    next =>            //    context =>
            //        context.NextHandler<THandler>().AsyncHandler(context.SetupData(context, next)));            return this;        }
        public IBotBuilder<TBot> Use<THandler>() where THandler : class, IAsyncHandler        {            Services.AddScoped<THandler>();            _components.Add(                next =>                context =>                     context.NextHandler<THandler>().AsyncHandler(context.SetupData(context, next)));            return this;        }


    }    internal abstract class BotBuilder : IBotBuilder    {        internal AsyncHandler HandlerDelegate { get; private set; }        internal abstract IServiceCollection Services { get; set; }        protected readonly ICollection<Func<AsyncHandler, AsyncHandler>> _components;        protected List<IBotCommandInfo> _botCommands = new List<IBotCommandInfo>();        internal BotBuilder()        {            _components = new List<Func<AsyncHandler, AsyncHandler>>();        }        public IBotBuilder Use(Func<AsyncHandler, AsyncHandler> middleware)        {            throw new NotImplementedException();        }        public IBotBuilder AddHandler<THandler>() where THandler : class, IAsyncHandler        {            Services.AddScoped<THandler>();            _components.Add(                next =>                context =>                     context.NextHandler<THandler>().AsyncHandler(context.SetupData(context, next)));            return this;        }        public IBotBuilder Use<THandler>(THandler handler) where THandler : class, IAsyncHandler        {            Services.AddScoped<THandler>();            _components.Add(next =>                context                    => handler.AsyncHandler(context.SetupData(context, next)));            return this;        }        public IBotBuilder Use(Func<IHandleContext, AsyncHandler> component)        {            throw new NotImplementedException();        }
        public AsyncHandler BuildHandler()        {
            AsyncHandler handle = context =>
                {
                    Update upd = context.Any<Update>();
#if DEBUG                    Console.WriteLine("No handler for update {0} of type {1}.", upd.UpdateId, upd.UpdateId);
#endif                    return Task.FromResult(1);
                };            foreach (var component in _components.Reverse())                handle = component(handle);            return HandlerDelegate = handle;        }


        #region Commands    


        public IBotBuilder Use<TCommand, THandler>() where THandler : BotCommandHandler<TCommand> where TCommand : class, IBotCommand        {                        Services.AddScoped<IAsyncHandler<TCommand>, THandler>();            Services.AddScoped<TCommand>();
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

        #endregion     }}