namespace MissBot.Abstractions
{
    public interface IResponse
    {
        Task SendHandlingStart();
        Task WriteAsync<T>(T data, CancellationToken cancel) where T : class;
    }
    public interface IResponseChannel
    {
      
    }

    public interface IResponseNotification
    {
        Task ShowPopupAsync(string message, CancellationToken cancel = default);
        Task SendTextAsync(string message, CancellationToken cancel = default);
    }

    public interface IResponse<TUnit> : IResponseChannel
    {
        Task Commit(CancellationToken cancel);
        void WriteMetadata<TMetaData>(TMetaData meta) where TMetaData : MetaData;
        void Write<TUnitData>(TUnitData unit) where TUnitData : Unit<TUnit>;
        void WriteError<TUnitData>(TUnitData unit) where TUnitData : Unit;
        void WriteResult<TUnitData>(TUnitData unit) where TUnitData : IEnumerable<Unit>;
        void Write<TUnitData>(IEnumerable<TUnitData> units) where TUnitData : Unit<TUnit>;
    }
}
