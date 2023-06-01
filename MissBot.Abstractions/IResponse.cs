using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Entities;
using MissBot.Entities;

namespace MissBot.Abstractions
{
    public interface IResponse
    {
        IResponse CompleteInput(string message);
        Task Commit(CancellationToken cancel = default);       
    }

    public interface IResponseNotification
    {
        Task ShowPopupAsync(string message, CancellationToken cancel = default);
        Task SendTextAsync(string message, CancellationToken cancel = default);
        Task Complete(CancellationToken cancel = default);
    }

    public interface IResponseError : IResponse, IBotRequest
    {
        IResponseError Write(Exception error);
    }

    public interface IResponse<TUnit> : IResponse where TUnit:class,IBotEntity
    {
        int Length { get; }
        IUnit<TUnit> Content { get; set; }
        void WriteMetadata<TData>(TData meta) where TData :class, IMetaData;
        void Write<TData>(TData unit) where TData:IUnit<TUnit>;//;= : IUnit<TUnit>;        
        void Write<TData>(IEnumerable<TData> units) where TData : TUnit;
        IResponse<TUnit> InputData(string description, IActionsSet options = default);
    }
}
