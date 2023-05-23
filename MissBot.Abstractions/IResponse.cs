using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions
{

    public interface IResponse
    {
        Task Commit(CancellationToken cancel);       
    }

    public interface IResponseNotification
    {
        Task ShowPopupAsync(string message, CancellationToken cancel = default);
        Task SendTextAsync(string message, CancellationToken cancel = default);
    }

    public interface IResponse<TUnit> : IResponse
    {
        void WriteMetadata<TData>(TData meta) where TData :class, IMetaData;
        void Write<TData>(TData unit) where TData : Unit, IUnit<TUnit>;
        void WriteError<TData>(TData unit) where TData : class, IUnit;
        void WriteResult<TData>(TData unit) where TData : IEnumerable<IUnit<TUnit>>;
        void Write<TData>(IEnumerable<TData> units) where TData : Unit, IUnit<TUnit>;
    }
}
