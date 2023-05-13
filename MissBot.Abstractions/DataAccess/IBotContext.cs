using System.Data.Common;
using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions.DataAccess
{
    public interface IBotContext
    {
        void LoadBotInfrastructure();
        DbConnection NewConnection();
        Task<int> HandleRequestCommandAsync(string sql, CancellationToken cancel = default);
    }
}
