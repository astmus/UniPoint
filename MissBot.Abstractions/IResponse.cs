using MissBot.Abstractions.Bot;
using MissBot.Entities;
using MissBot.Entities.Abstractions;

namespace MissBot.Abstractions
{
    public interface IResponse
    {
        int Length { get; }
        IResponse Write(object data);
        Task Commit(CancellationToken cancel = default);
    }

    public interface IResponseNotification
    {
        Task ShowPopupAsync(string message, CancellationToken cancel = default);
        Task SendTextAsync(string message, CancellationToken cancel = default);
        Task Complete(CancellationToken cancel = default);
    }

    public interface IInteractiveResponse : IResponse, IInteractible
    {
        IResponse InputDataInteraction(string description, IActionsSet options = default);
        IResponse CompleteInteraction(object completeObject);
    }

    public interface IInteractible
    {
        IActionsSet ActionsSet { get; set; }
    }

    public interface IInteraction<TData> : IResponse, IInteractiveResponse where TData : class
    {
    }

    public interface IResponseError : IResponse, IBotRequest
    {
        IResponseError Write(Exception error);
    }

    public interface IResponse<in TUnit> : IResponse where TUnit : class
    {
        void WriteMetadata<TData>(TData meta) where TData : class, IMetaData;
        void WriteUnit<TData>(TData unit) where TData : class, IUnit<TUnit>;
        void AddUnit<TData>(IUnit<TData> unit) where TData : class, TUnit;
        void AddUnits<TData>(IEnumerable<TData> units) where TData : class, IUnit<TUnit>;
    }
}
