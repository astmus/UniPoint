

using System.Collections;
using System.Collections.Generic;

namespace MissBot.Abstractions
{
    public interface IResponseChannel 
    {
           
    }

    public interface IResponse<TUnit> : IResponseChannel
    {   
        //IEnumerable<TUnit> Units { get; }
        Task Commit(CancellationToken cancel);
        void Init(ICommonUpdate update, BotClientDelegate sender);
        Task<IResponseChannel> InitAsync(TUnit data, ICommonUpdate update, BotClientDelegate sender);
        //Task WriteAsync<T>(T data, CancellationToken cancel) where T : class;
        //Task UpdateAsync<T>(T data, CancellationToken cancel) where T : class;
        //Task SendAsync(TData data, CancellationToken cancel);        
        void Write<TUnitData>(TUnitData unit) where TUnitData : BaseEntity<TUnit>.Union;
    }
}
