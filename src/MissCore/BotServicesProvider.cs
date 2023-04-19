using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using Telegram.Bot.Types;

namespace MissCore
{
    public class BotServicesProvider : IBotServicesProvider
    {
        IServiceProvider sp;
        public BotServicesProvider(IServiceProvider spr)
            => sp = spr;

        public IBotClient Client
            => GetRequiredService<IBotClient>();

        //public TCommand GetCommand<TCommand>() where TCommand : BotCommand
        //{
        //    var repository = sp.GetRequiredService<IRepository<BotCommand>>();
        //    repository.Get
        //}

        public T GetRequiredService<T>()
            => sp.GetRequiredService<T>();

        public T GetService<T>()
            => sp.GetService<T>();

        public object? GetService(Type serviceType)
            => sp.GetService(serviceType);

        public IResponse<T> Response<T>()
            => sp.GetRequiredService<IResponse<T>>();
    }
}
