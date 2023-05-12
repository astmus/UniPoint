using Microsoft.Extensions.Configuration;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.DataModel;
using MissBot.DataAccess.Interfacet;
using MissBot.DataAccess.Sql;
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
        public BotDataContext Context { get; }

        protected  string GetConnectionString()
            => config.GetConnectionString("Default");
        public BotRepository(IConfiguration configuration,/* IDataConnection dataProvider,*/ BotDataContext context)// : base(new BotContextOptions(configuration.GetConnectionString("Default")))
        {
            config = configuration;
           // this.DataProvider = dataProvider;
            Context = context;
        }

        public BotRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        //public async Task<TResult> HandleSqlQueryAsync<TResult>(IRepositoryCommand sql, CancellationToken cancel = default) where TResult : class
        //{
        //    return await base.HandleQueryAsync<TResult>(sql, cancel);
        //}


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
            using (var connection = Context.DataProvider.CreateConnection(GetConnectionString()))
            {

                await connection.OpenAsync();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query.Command;
                    try
                    {
                        if (await cmd.ExecuteScalarAsync(cancel).ConfigureAwait(false) is string res)
                            result = JsonConvert.DeserializeObject<TResult>(res);
                        await connection.CloseAsync();
                    }
                    catch
                    {
                        await connection.CloseAsync();
                        throw;
                    }
                }
            }
            return result;
        }


        //public void InitContext(IBotDataContext dataContext)
        //{
        //    throw new NotImplementedException();
        //}

        //public IBotConnection CreateConnection(string connectionString)
        //{
        //    throw new NotImplementedException();
        //}

        //public object? GetConnectionInfo(IDataConnection dataConnection, string parameterName)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

