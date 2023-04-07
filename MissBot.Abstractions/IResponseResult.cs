

using System.Collections;
using System.Collections.Generic;

namespace MissBot.Abstractions
{
    public interface IResponseResult 
    {
           
    }

    public interface IResponseResult<TUnit> : IResponseResult, IEnumerable<TUnit>
    {        
        IResponseResult<TUnit> CreateChannel();
        //IEnumerable<TUnit> Units { get; }
        Task Commit(CancellationToken cancel);
        //Task WriteAsync<T>(T data, CancellationToken cancel) where T : class;
        //Task UpdateAsync<T>(T data, CancellationToken cancel) where T : class;
        //Task SendAsync(TData data, CancellationToken cancel);        
        void Write<TUnitData>(TUnitData unit) where TUnitData : TUnit;
    }
}
