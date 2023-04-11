namespace MissBot.Abstractions
{
    public interface IResponse
    {
        Task SendHandlingStart();        
        Task WriteAsync<T>(T data, CancellationToken cancel) where T : class;
        Task<IResponseChannel> InitAsync<T>(T data, CancellationToken cancel, IHandleContext context) where T : class;
        IResponse<T> Init<T>(T data, IHandleContext context);
    }
    public interface IResponseChannel
    {

    }

    public interface IResponse<TUnit> : IResponseChannel
    {
        Task Commit(CancellationToken cancel);
        void Init(ICommonUpdate update, BotClientDelegate sender, TUnit unit = default);
        Task<IResponseChannel> InitAsync(TUnit data, ICommonUpdate update, BotClientDelegate sender);
        void Write<TUnitData>(TUnitData unit) where TUnitData : BaseEntity.BotUnit;
        void WriteResult<TUnitData>(TUnitData unit) where TUnitData : BaseEntity.BotUnion;
        void Write<TUnitData>(IEnumerable<TUnitData> units) where TUnitData : Unit<TUnit>;
    }
}
