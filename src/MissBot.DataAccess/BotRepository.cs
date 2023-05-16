using Microsoft.Extensions.Configuration;
using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using Newtonsoft.Json;

namespace MissBot.DataAccess
{
    public class BotRepository : IBotRepository
    {
        private readonly IConfiguration config;
        private IConfiguration configuration;

        public string Name { get; }
        public int ID { get; }
        public string? ConnectionNamespace { get; }
        public IDataConnection DataProvider { get; }
        public IBotContext Context { get; }
        public IEnumerable<BotCommand> Commands { get; }

        public BotRepository(IBotContext context)// : base(new BotContextOptions(configuration.GetConnectionString("Default")))
        {
            Context = context;
        }

        public Task ExecuteCommandAsync(IRepositoryCommand query, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public async Task<TResult> HandleQueryAsync<TResult>(IRepositoryCommand query, CancellationToken cancel = default) where TResult : class
        {
            return await HandleCommandAsync<TResult>(query, cancel);
        }
        public async Task<TResult> HandleCommandAsync<TResult>(IRepositoryCommand query, CancellationToken cancel = default)
        {
            TResult result = default(TResult);
            using (var connection = Context.NewConnection())
            {
                await connection.OpenAsync(cancel);
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query.ToRequest();
                    try
                    {
                        if (await cmd.ExecuteScalarAsync(cancel).ConfigureAwait(false) is string res)
                            result = JsonConvert.DeserializeObject<TResult>(res);
                    }
                    finally
                    {
                        await connection.CloseAsync();
                    }
                }
            }
            return result;
        }
    }
}

