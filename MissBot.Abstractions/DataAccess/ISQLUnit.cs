namespace MissBot.Abstractions.DataContext
{

    public interface IQueryUnit<in TResultUnit>
    {
        Task<TResult> GetResponseAsync<TResult>(IRepository repository, CancellationToken cancel = default) where TResult : TResultUnit;
        Task<ICollection<TResult>> GetResponseItemsAsync<TResult>(IRepository repository, CancellationToken cancel = default) where TResult : TResultUnit;
    }

}
