using TG = Telegram.Bot.Types;
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

    public interface IResponseNotification
    {
        Task ShowPopupAsync(string message, CancellationToken cancel = default);
        Task SendTextAsync(string message, CancellationToken cancel = default);
        void Init(ICommonUpdate update, BotClientDelegate sender, TG.CallbackQuery unit);        
    }

    public interface IResponse<TUnit> : IResponseChannel
    {
        Task Commit(CancellationToken cancel);
        void Init(ICommonUpdate update, BotClientDelegate sender, TUnit unit = default);
        Task<IResponseChannel> InitAsync(TUnit data, ICommonUpdate update, BotClientDelegate sender);
        void WriteMetadata<TMetaData>(TMetaData meta) where TMetaData : Unit<TUnit>.MetaUnit;
        void Write<TUnitData>(TUnitData unit) where TUnitData : ValueUnit;
        void WriteResult<TUnitData>(TUnitData unit) where TUnitData : IEnumerable<ValueUnit> ;
        void Write<TUnitData>(IEnumerable<TUnitData> units) where TUnitData : ValueUnit;
    }
}
