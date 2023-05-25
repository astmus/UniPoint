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
    }

    public interface IErrorResponse : IResponse, IBotRequest
    {
        IErrorResponse Write(Exception error);
    }

    public interface IResponse<TUnit> : IResponse
    {
        int Length { get; }
        string Content { get; set; }
        void WriteMetadata<TData>(TData meta) where TData :class, IMetaData;
        void Write<TData>(TData unit) where TData : Unit, IUnit<TUnit>;        
        void WriteResult<TData>(TData unit) where TData : IEnumerable<IUnit<TUnit>>;
        void Write<TData>(IEnumerable<TData> units) where TData : Unit, IUnit<TUnit>;
        IResponse<TUnit> InputData(string description, IActionsSet options = default);
    }
}
