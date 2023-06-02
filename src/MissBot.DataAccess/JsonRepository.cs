using System.Data;
using System.Data.Common;
using System.Text;
using LinqToDB;
using Microsoft.Extensions.Options;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissCore.Bot;
using MissCore.Data;
using MissCore.Data.Collections;
using MissCore.DataAccess;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MissBot.DataAccess
{
    public class JsonRepository : DataContext, IJsonRepository
    {
        
        public JsonRepository(IOptions<BotContextOptions> ctxOptions) : base(ctxOptions.Value.DataProvider, ctxOptions.Value.ConnectionString)
        {
        }

        public Task ExecuteCommandAsync(IUnitRequest request, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IMetaCollection<TUnit>> FindAsync<TUnit>(ISearchUnitRequest<TUnit> query, CancellationToken cancel = default) where TUnit : BaseUnit
        {
            if (await HandleRawAsync(JArray.Parse, query.GetCommand(), cancel) is JArray items)
                return new MetaCollection<TUnit>(items);
            else
                return MetaCollection<TUnit>.Empty;
        }

        public async Task<TResult> HandleQueryAsync<TResult>(IUnitRequest request, CancellationToken cancel = default) where TResult : class
        {
            TResult result = default(TResult);
            using (var connection = DataProvider.CreateConnection(ConnectionString))
            {
                await connection.OpenAsync(cancel);
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = request.GetCommand();
                   
                    if (await cmd.ReadAsync(cancel) is string json)
                        result = JsonConvert.DeserializeObject<TResult>(json);
                }
                await connection.CloseAsync().ConfigureAwait(false);
            }
            return result;
        }
        public async Task<IMetaCollection> ReadAsync(IUnitRequest cmd, CancellationToken cancel = default)
        {
            var array = await HandleRawAsync(JArray.Parse, cmd.GetCommand(), cancel);
            return new MetaCollection(array);
        }

        public async Task<JObject> HandleScalarAsync(IUnitRequest cmd, CancellationToken cancel = default)
        {
            var jobj = await HandleRawAsync(JObject.Parse, cmd.GetCommand(), cancel);
            return jobj;
        }

        public async Task<ICollection<TResult>> ReadCollectionAsync<TResult>(IUnitRequest cmd, CancellationToken cancel = default) where TResult : class
        {
            return await HandleQueryAsync<Unit<TResult>.Collection>(cmd, cancel);
        }

        protected virtual async Task<TJToken> HandleRawAsync<TJToken>(Func<string, TJToken> parser, string sql, CancellationToken cancel = default) where TJToken : JToken
        {
            TJToken result = default;
            using (var conn = DataProvider.CreateConnection(ConnectionString))
            {
                await conn.OpenAsync(cancel);
                try
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        if (await cmd.ReadAsync(cancel) is string json && json != string.Empty)
                            result = parser(json);
                    }
                }
                finally
                {
                    await conn.CloseAsync().ConfigureAwait(false);
                }
            }
            return result;
        }       

        public async Task<TResult> HandleScalarAsync<TResult>(IUnitRequest request, CancellationToken cancel = default)
        {
            TResult result = default;
            using (var conn = DataProvider.CreateConnection(ConnectionString))
            {
                try
                {
                    await conn.OpenAsync(cancel);
                    using (var cmd = conn.CreateCommand())
                    {
                        request.Options |= RequestOptions.JsonAuto | RequestOptions.Scalar;
                        cmd.CommandText = request.GetCommand();

                        cmd.LoadParameters(request.Params);

                        if (await cmd.ReadAsync(cancel) is string json && json.Length > 0)
                            result = JsonConvert.DeserializeObject<TResult>(json);
                        if (result is BaseUnit unit)
                            unit.InitializeMetaData();
                    }
                }
                catch(Exception error)
                {
                    throw error;
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
            return result;
        }

        public async Task<TResult> HandleRawAsync<TResult>(string request, CancellationToken cancel = default) where TResult : class
        {
            IUnitRequest command = new UnitRequest<TResult>(request);
            command.Options  |= RequestOptions.Scalar;
            JObject item = await HandleScalarAsync(command, cancel);
            var result = item?.ToObject<TResult>();
            if (result is BaseUnit unit)
                unit.InitializeMetaData();
            return result;
        }

        public async Task<IMetaCollection<TUnit>> HandleQueryAsync<TUnit>(IUnitRequest<TUnit> query, CancellationToken cancel = default) where TUnit : BaseUnit
        {
            var items = await HandleRawAsync(JArray.Parse, query.GetCommand(), cancel);

            return new MetaCollection<TUnit>(items);
        }

        public async Task<IContentUnit<TResult>> RawAsync<TResult>(string request, CancellationToken cancel = default, params KeyValuePair<object, object>[] parameters) where TResult : class
        {
            ContentUnit<TResult> result = null;
            using (var conn = DataProvider.CreateConnection(ConnectionString))
            {
                try
                {
                    await conn.OpenAsync(cancel);
                    using (var cmd = conn.CreateCommand())
                    {

                        cmd.CommandText = $"{request} {(RequestOptions.JsonAuto | RequestOptions.RootContent).Format()}";

                        cmd.LoadParameters(parameters);

                        if (await cmd.ReadAsync(cancel) is string json)
                            result = JsonConvert.DeserializeObject<ContentUnit<TResult>>(json);
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
            return result;
        }

    }
}
internal static class DBExtension
{
    internal static async Task<string> ReadAsync(this DbCommand cmd, CancellationToken cancel)
    {
        using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess, cancel))
        {
            StringBuilder b = new StringBuilder();
            while (await reader.ReadAsync(cancel))
                b.Append(await reader.GetTextReader(0).ReadToEndAsync(cancel));
            reader.Close();
            return b.ToString();
        }
    }
    internal static void LoadParameters(this DbCommand cmd, KeyValuePair<object, object>[] parameters)
    {
        foreach (var arg in parameters)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = (string)arg.Key;
            p.Value = arg.Value;
            cmd.Parameters.Add(p);
        }
    }
    internal static void LoadParameters(this DbCommand cmd, IEnumerable<IUnitItem> parameters)
    {
        foreach (var arg in parameters)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = "@"+arg.ItemName;
            p.Value = arg.ItemValue;
            cmd.Parameters.Add(p);
        }
    }
}
