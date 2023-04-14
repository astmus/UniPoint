
using System.Runtime.CompilerServices;
using System.Text.Json;
using BotService;
using MediatR;
using Microsoft.Data.SqlClient;
using MissBot.Abstractions;
using Newtonsoft.Json;

namespace MissDataMaiden.Queries
{
    public record SqlBotQuery<TEntity> : IRequest<BotUnion<TEntity>> where TEntity : class
    {
        public record Query(string sql, int skip, int take, string filter) : SqlBotQuery<TEntity>;
        public class Handler<TQuery> : IRequestHandler<TQuery, BotUnion<TEntity>> where TQuery : SqlBotQuery<TEntity>.Query
        {
            string connectionString;
            public Handler(IConfiguration config)
                => connectionString = config.GetConnectionString("Default");
            public async Task<BotUnion<TEntity>> Handle(TQuery request, CancellationToken cancellationToken)
            {
                var queryWithForJson = string.Format(request.sql, request.skip, request.take, request.filter);

                BotUnion<TEntity> single = new BotUnion<TEntity>();
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
                            {
                                TEntity item = default(TEntity);
                                item = Newtonsoft.Json.JsonConvert.DeserializeObject<TEntity>(reader.GetString(0));

                                single.Add(CreateUnit(item));
                            }
                    }
                    return single;
                }
            }

            protected virtual Unit<TEntity> CreateUnit(TEntity entity)
                => Unit<TEntity>.Instance with { Value = entity };

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
