namespace MissBot.Abstractions
{
    public interface IResponse
    {
        Task SendHandlingStart();
        void SetContext(IHandleContext context);
        Task WriteAsync<T>(T data, CancellationToken cancel) where T : class;
        Task<IResponseChannel> CreateAsync<T>(T data, CancellationToken cancel) where T : class;
        IResponse<T> Create<T>(T data) where T : class;
    }
    public interface IResponseChannel
    {

    }

    public interface IResponse<TUnit> : IResponseChannel
    {
        Task Commit(CancellationToken cancel);
        void Init(ICommonUpdate update, BotClientDelegate sender, TUnit unit = default);
        Task<IResponseChannel> InitAsync(TUnit data, ICommonUpdate update, BotClientDelegate sender);
        void Write<TUnitData>(TUnitData unit) where TUnitData : Unit<TUnit>;
        void Write<TUnitData>(IEnumerable<TUnitData> units) where TUnitData : Unit<TUnit>;
    }
}
