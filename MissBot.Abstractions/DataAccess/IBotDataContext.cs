namespace MissBot.Abstractions.DataModel
{
    public interface IBotDataContext
    {
        Task<int> HandleRequestCommandAsync(string sql, CancellationToken cancel = default);
    }
}
