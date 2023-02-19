using MissBot.Abstractions;
using MissBot.Handlers;
using MissBot.Response;
using MissCore;
using MissCore.Abstractions;using MissCore.Configuration;using MissCore.Data.Context;
using MissCore.Entities;namespace BotService.Internal{    internal class BotBuilder<TBot> : BotBuilder, IBotBuilder<TBot> where TBot : class, IBot    {        internal static BotBuilder<TBot> instance;        internal static BotBuilder<TBot> Instance { get => instance; }        internal override IServiceCollection Services { get; set; }        public IServiceCollection BotServices            => Services;        static IHostBuilder host;        internal static BotBuilder<TBot> GetInstance(IHostBuilder rootHost)        {            host = rootHost;            host.ConfigureServices((h, s) => { instance.Services.AddTransient(sp => h.Configuration); });            return Instance;        }        static BotBuilder()        {            instance = new BotBuilder<TBot>();            instance.Services = new ServiceCollection();
        }        internal BotBuilder()
        {            Services = Instance?.Services;        }        public IBotBuilder<TBot> UseCommndFromAttributes()        {            var cmds = typeof(TBot).GetCommandsFromAttributes();            _botCommands.AddRange(cmds);            _botCommands.ForEach(c => Services.AddScoped(c.CmdType));            return this;        }        IBotServicesProvider sp;        public IBotServicesProvider BotServicesProvider()            => sp ??= new BotServicesProvider(Instance.Services.BuildServiceProvider());        public IBotBuilder<TBot> UseUpdateHandler<THandler>() where THandler : class, IAsyncHandler<Update<TBot>>        {            Services.AddTransient<IAsyncHandler<Update<TBot>>, THandler>();            return this;        }        public IBotBuilder<TBot> UseContextHandler<THandler>() where THandler : class, IContextHandler<Update<TBot>>        {            Services.AddSingleton<THandler>();
            Services.AddScoped<IBotBuilder<TBot>>(sp => BotBuilder<TBot>.Instance);            host.ConfigureServices((h, s) => s.AddSingleton<IContextHandler<Update<TBot>>, THandler>());
            _components.Add(                next =>                context =>
                    context.NextHandler<THandler>().ExecuteAsync(context, next)
            );            return this;        }


    }    internal abstract class BotBuilder : IBotBuilder    {        internal HandleDelegate HandlerDelegate { get; private set; }        internal abstract IServiceCollection Services { get; set; }        protected readonly ICollection<Func<HandleDelegate, HandleDelegate>> _components;        protected List<IBotCommandInfo> _botCommands = new List<IBotCommandInfo>();        internal BotBuilder()        {            _components = new List<Func<HandleDelegate, HandleDelegate>>();        }        public IBotBuilder Use(Func<HandleDelegate, HandleDelegate> middleware)        {            throw new NotImplementedException();        }        public IBotBuilder Use<THandler>() where THandler : class, IAsyncHandler        {            Services.AddScoped<THandler>();            _components.Add(                next =>                context =>                     context.NextHandler<THandler>().ExecuteAsync(context, next)            );            return this;        }        public IBotBuilder Use<THandler>(THandler handler) where THandler : class, IAsyncHandler        {            Services.AddScoped<THandler>();            _components.Add(next =>                context                    => handler.ExecuteAsync(context, next)            );            return this;        }        public IBotBuilder Use(Func<IHandleContext, HandleDelegate> component)        {            throw new NotImplementedException();        }        public HandleDelegate BuildHandler()        {
            HandleDelegate handle = context =>
                {
                    context.Update.IsHandled = true;
#if DEBUG                    Console.WriteLine("No handler for update {0} of type {1}.", context.Update.UpdateId, context.Update);
#endif                    return Task.FromResult(1);
                };            foreach (var component in _components.Reverse())                handle = component(handle);            return HandlerDelegate = handle;        }


        #region Commands    


        public IBotBuilder Use<TCommand, THandler>() where THandler : class, IAsyncHandler<TCommand>, IAsyncBotCommansHandler where TCommand : class, IBotCommandData        {            Services.AddScoped<IAsyncHandler<TCommand>, THandler>();            Services.AddScoped<TCommand>();
            Services.AddScoped<IResponseChannel, ResponseChannel>();
            Services.AddScoped<IContext<TCommand>, Context<TCommand>>();
            //_components.Add(
            //    next =>
            //    context =>
            //         context.NextHandler<THandler>().ExecuteAsync(context, next));
            return this;        }

        public void Build()
        {
            if (HandlerDelegate is null)
                BuildHandler();
        }









        #endregion                                                                                }}