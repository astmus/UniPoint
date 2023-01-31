using MissCore.Abstractions;
using MissCore.Entities;
using MissCore.Handlers;

namespace MissCore.Abstractions
{
    public interface IBotHandler<T> : IAsyncHandler<Update<T>> where T: IBot
    {        
        Task StartHandle(IContext<T> context, Update<T> update);
    }
}
