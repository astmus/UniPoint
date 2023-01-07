
using System.Text;
using BotService;
using MediatR;
using Microsoft.Data.SqlClient;

namespace MissDataMaiden.Queries
{
    public record SqlRawQuery(string sql) : IRequest<string>;


    internal class SqlQueryHandler : IRequestHandler<SqlRawQuery, string>
    {
        string connectionString;
        public SqlQueryHandler(IConfiguration config)
            => connectionString = config.GetConnectionString("Default");
        public async Task<string> Handle(SqlRawQuery request, CancellationToken cancellationToken)
        {
            var queryWithForJson = $"{request.sql} FOR JSON AUTO";
            string res;
            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(queryWithForJson, conn))
                {
                    await conn.OpenAsync(cancellationToken).ConfigFalse();
                    var jsonResult = new StringBuilder();
                    var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                    if (!reader.HasRows)
                        jsonResult.Append("[]");
                    else
                    {
                        while (await reader.ReadAsync())
                            jsonResult.Append(reader.GetValue(0).ToString());
                    }
                    res = jsonResult.ToString();
                }
                return res;
            }
        }
    }
}
