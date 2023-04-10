
using System.Runtime.CompilerServices;
using System.Text.Json;
using BotService;
using MediatR;
using Microsoft.Data.SqlClient;
using MissBot.Abstractions;

namespace MissDataMaiden.Queries
{
    public record SqlQuery<TUnit>(string sql, int skip, int take) : IRequest<IEnumerable<TUnit>> where TUnit : BaseEntity { }
    public record SqlRaw<TUnit> : IStreamRequest<TUnit> where TUnit:BaseEntity{
        public record Query(string sql, string connection = null) : SqlRaw<TUnit>;
        public class StreamHandler<TQuery> : IStreamRequestHandler<TQuery, TUnit> where TQuery:SqlRaw<TUnit>.Query
        {
            string connectionString;
            public StreamHandler(IConfiguration config)
                => connectionString = config.GetConnectionString("Default");
            public async IAsyncEnumerable<TUnit> Handle(TQuery request, [EnumeratorCancellation] CancellationToken cancellationToken)
            {
                var queryWithForJson = $"{request.sql}";

                TUnit single;
                using (var conn = new SqlConnection(connectionString))
                {
                    using (var cmd = new SqlCommand(queryWithForJson, conn))
                    {
                        await conn.OpenAsync(cancellationToken).ConfigFalse();

                        var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                        
                        if (!reader.HasRows)
                            single = Newtonsoft.Json.JsonConvert.DeserializeObject<TUnit>(reader.GetString(0));
                        else
                        {
                            while (await reader.ReadAsync())
                                yield return Newtonsoft.Json.JsonConvert.DeserializeObject<TUnit>(reader.GetString(0)); //System.Text.Json.JsonSerializer.Deserialize<TUnit>(reader.GetString(0),options);
                            yield break;
                        }

                    }
                    yield return single;
                }
            }
        };
        public class Handler<TQuery> : IRequestHandler<TQuery, IEnumerable<TUnit>> where TQuery : SqlQuery<TUnit>
        {
            string connectionString;
            public Handler(IConfiguration config)
                => connectionString = config.GetConnectionString("Default");
            public async Task<IEnumerable<TUnit>> Handle(TQuery request, CancellationToken cancellationToken)
            {
                var queryWithForJson = string.Format(request.sql,request.skip,request.take);

                List<TUnit> single =  new List<TUnit>();
                using (var conn = new SqlConnection(connectionString))
                {
                    using (var cmd = new SqlCommand(queryWithForJson, conn))
                    {
                        await conn.OpenAsync(cancellationToken).ConfigFalse();

                        var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                        if (!reader.HasRows)
                            single.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<TUnit>(reader.GetString(0)));
                        else
                            while (await reader.ReadAsync())
                                single.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<TUnit>(reader.GetString(0)));
                    }
                    return single;
                }
            }

      
        }       
    }
}
