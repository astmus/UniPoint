using System.Collections.Generic;
using MissCore.Abstractions;

namespace MissCore.DataAccess.Async
{
    public interface IBotUpdatesReceiver<out TUpdate> : IAsyncEnumerable<TUpdate> where TUpdate : class, IUpdateInfo
    {

    }

    public interface IAsyncUpdatesQueue<in TUpdate> where TUpdate :  IUpdateInfo
    {
        void PushUpdate(TUpdate updade);
    }
}
