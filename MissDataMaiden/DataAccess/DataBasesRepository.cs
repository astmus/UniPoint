using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotService;
using MediatR;
using Microsoft.Data.SqlClient;
using MissBot.Abstractions;
using MissBot.DataAccess;
using MissDataMaiden.Entities;
using MissDataMaiden.Queries;

namespace MissDataMaiden.DataAccess
{
    internal class DataBasesRepository : BaseRepository<DataBase>
    {
        public record SqlQuery(string sql, string id) : IRequest<DataBase>;
        public class Handler : IRequestHandler<SqlQuery, DataBase>
        {
            string connectionString;
            public Handler(IConfiguration config)
                => connectionString = config.GetConnectionString("Default");
            public async Task<DataBase> Handle(SqlQuery request, CancellationToken cancellationToken)
            {
                var queryWithForJson = string.Format(request.sql, request.id);
                DataBase single = null;
                using (var conn = new SqlConnection(connectionString))
                {
                    using (var cmd = new SqlCommand(queryWithForJson, conn))
                    {
                        await conn.OpenAsync(cancellationToken).ConfigFalse();

                        var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                        if (!reader.HasRows)
                            return single;//single.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<TUnit>(reader.GetString(0)));
                        else
                            while (await reader.ReadAsync())
                                single = Newtonsoft.Json.JsonConvert.DeserializeObject<DataBase>(reader.GetString(0));
                    }
                    return single;
                }
            }
        }
         public override Task<DataBase> GetAsyncForAction<TAction>(TAction action)
        {
            
            return Task.FromResult(new DataBase() { Id = Convert.ToInt32(action.Id) });
        }
    }
}
