
using MissBot.Abstractions.Configuration;

namespace MissBot.Abstractions
{
    public interface IBotServicesProvider : IServiceProvider
    {
        IBotClient Client { get; }
        //TCommand GetCommand<TCommand>() where TCommand : BotCommand;
        IResponse<T> Response<T>();
        T GetService<T>();
        T GetRequiredService<T>();
    }
}
