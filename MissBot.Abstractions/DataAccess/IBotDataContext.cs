using System.Data.Common;
using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions.DataModel
{
    public interface IBotDataContext
    {
        void LoadBotInfrastructure();
        DbConnection NewConnection();        
        Task<IEnumerable<BotCommand>> LoadCommandsAsync();
        Task<int> HandleRequestCommandAsync(string sql, CancellationToken cancel = default);
    }
}
