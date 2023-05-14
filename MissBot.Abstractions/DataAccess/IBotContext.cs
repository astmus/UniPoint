using System.Data.Common;

namespace MissBot.Abstractions.DataAccess
{
    public interface IBotContext
    {
        void LoadBotInfrastructure();
        DbConnection NewConnection();
        Task<TResult> HandleRequestAsync<TResult>(SQLCommand sql, CancellationToken cancel = default);
    }
}
