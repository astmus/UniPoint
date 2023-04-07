
using System.Runtime.CompilerServices;
using System.Text.Json;
using BotService;
using MediatR;
using Microsoft.Data.SqlClient;
using MissBot.Abstractions;

namespace MissDataMaiden.Queries
{

    public record SqlRaw<TUnit> : IStreamRequest<TUnit> where TUnit:BaseEntity{
        public record Query(string sql, string connection = null) : SqlRaw<TUnit>;
        public class Handler<TQuery> : IStreamRequestHandler<TQuery, TUnit> where TQuery:SqlRaw<TUnit>.Query
        {
            string connectionString;
            public Handler(IConfiguration config)
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
    //internal class SqlRawQueryHandler<TUnit> : IStreamRequestHandler<SqlRaw<TUnit>.Query, TUnit> where TUnit:class
    //{
    //    string connectionString;
    //    public SqlRawQueryHandler(IConfiguration config)
    //        => connectionString = config.GetConnectionString("Default");
    //    public async IAsyncEnumerable<TUnit> Handle(SqlRaw<TUnit>.Query request, [EnumeratorCancellation] CancellationToken cancellationToken)
    //    {
    //        var queryWithForJson = $"{request.sql}";

    //        TUnit single;
    //        using (var conn = new SqlConnection(connectionString))
    //        {
    //            using (var cmd = new SqlCommand(queryWithForJson, conn))
    //            {
    //                await conn.OpenAsync(cancellationToken).ConfigFalse();
     
    //                var reader = await cmd.ExecuteReaderAsync(cancellationToken);
    //                if (!reader.HasRows)
    //                    single = System.Text.Json.JsonSerializer.Deserialize<TUnit>(reader.GetString(0));
    //                else
    //                {
    //                    while (await reader.ReadAsync())
    //                        yield return System.Text.Json.JsonSerializer.Deserialize<TUnit>(reader.GetString(0));
    //                    yield break;
    //                }

    //            }
    //            yield return single;
    //        }
    //    }
        //internal class SqlQueryHandler : IStreamRequestHandler<SqlRawQuery, BaseUnit>
        //{
        //    string connectionString;
        //    public SqlQueryHandler(IConfiguration config)
        //        => connectionString = config.GetConnectionString("Default");
        //    public async IAsyncEnumerable<BaseUnit> Handle(SqlRawQuery request,[EnumeratorCancellation] CancellationToken cancellationToken)
        //    {
        //        var queryWithForJson = $"{request.sql}";
        //        string res;
        //                string single;
        //        using (var conn = new SqlConnection(connectionString))
        //        {
        //            using (var cmd = new SqlCommand(queryWithForJson, conn))
        //            {
        //                await conn.OpenAsync(cancellationToken).ConfigFalse();
        //                var jsonResult = new StringBuilder();
        //                var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        //                if (!reader.HasRows)
        //                    single = reader.GetString(0);
        //                else
        //                {
        //                    while (await reader.ReadAsync())
        //                        yield return reader.GetString(0);
        //                    yield break;
        //                }
        //                res = jsonResult.ToString();
        //            }
        //            yield return single;
        //        }
        //    }
    }
}
