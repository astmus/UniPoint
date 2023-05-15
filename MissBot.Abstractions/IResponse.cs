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
        void WriteMetadata<TMetaData>(TMetaData meta) where TMetaData :class, IMetaData;
        void Write<TUnitData>(TUnitData unit) where TUnitData : class, IUnit<TUnit>;
        void WriteError<TUnitData>(TUnitData unit) where TUnitData : class, IUnit;
        void WriteResult<TUnitData>(TUnitData unit) where TUnitData : IEnumerable<IUnit>;
        void Write<TUnitData>(IEnumerable<TUnitData> units) where TUnitData : class, IUnit<TUnit>;
    }
}
