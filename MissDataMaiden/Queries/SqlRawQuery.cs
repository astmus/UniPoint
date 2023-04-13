
using System.Runtime.CompilerServices;
using System.Text.Json;
using BotService;
using MediatR;
using Microsoft.Data.SqlClient;
using MissBot.Abstractions;

namespace MissDataMaiden.Queries
{
    public record SqlQuery<TUnit> : IRequest<BotUnion<TUnit>> where TUnit : class {
        public record Query(string sql, int skip, int take, string filter) : SqlQuery<TUnit>;
        public class Handler<TQuery> : IRequestHandler<TQuery, BotUnion<TUnit>> where TQuery : SqlQuery<TUnit>.Query
        {
            string connectionString;
            public Handler(IConfiguration config)
                => connectionString = config.GetConnectionString("Default");
            public async Task<BotUnion<TUnit>> Handle(TQuery request, CancellationToken cancellationToken)
            {
                var queryWithForJson = string.Format(request.sql, request.skip, request.take, request.filter);

                BotUnion<TUnit> single =new BotUnion<TUnit>();
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
                                single.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<TUnit>(reader.GetString(0)));
                    }
                    return single;
                }
            }

        }
        }
    public record SqlRaw<TUnion> : IStreamRequest<TUnion> where TUnion : ValueUnit
    {
        public record Query(string sql, string connection = null) : SqlRaw<TUnion>;
        public class StreamHandler<TQuery> : IStreamRequestHandler<TQuery, TUnion> where TQuery : SqlRaw<TUnion>.Query
        {
            string connectionString;
            public StreamHandler(IConfiguration config)
                => connectionString = config.GetConnectionString("Default");
            public async IAsyncEnumerable<TUnion> Handle(TQuery request, [EnumeratorCancellation] CancellationToken cancellationToken)
            {
                var queryWithForJson = $"{request.sql}";

                TUnion single;
                using (var conn = new SqlConnection(connectionString))
                {
                    using (var cmd = new SqlCommand(queryWithForJson, conn))
                    {
                        await conn.OpenAsync(cancellationToken).ConfigFalse();

                        var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                        if (!reader.HasRows)
                            single = Newtonsoft.Json.JsonConvert.DeserializeObject<TUnion>(reader.GetString(0));
                        else
                        {
                            while (await reader.ReadAsync())
                                yield return Newtonsoft.Json.JsonConvert.DeserializeObject<TUnion>(reader.GetString(0)); //System.Text.Json.JsonSerializer.Deserialize<TUnit>(reader.GetString(0),options);
                            yield break;
                        }

                    }
                    yield return single;
                }
            }
        };
    }
}
