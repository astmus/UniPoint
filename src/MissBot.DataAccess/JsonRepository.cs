using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Text;
using LinqToDB;
using LinqToDB.Reflection;
using Microsoft.Extensions.Options;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissCore.Bot;
using MissCore.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MissBot.DataAccess
{
    public class JsonRepository : DataContext, IJsonRepository
    {
        IRequestProvider provider;
        public JsonRepository(IOptions<BotContextOptions> ctxOptions, IRequestProvider requestProvider) : base(ctxOptions.Value.DataProvider, ctxOptions.Value.ConnectionString)
        {
            provider = requestProvider;
        }

        public Task ExecuteCommandAsync(IUnitRequest request, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IMetaCollection<TUnit>> FindAsync<TUnit>(ISearchUnitRequest<TUnit> query, CancellationToken cancel = default) where TUnit : UnitBase
        {
            //var request = provider.FindRequest<TUnit>(search, skip, take);

            var items = await HandleRawAsync(query.GetCommand(), cancel);

            return new MetaCollection<TUnit>(items);
        }


        public Task<TResult> HandleCommandAsync<TResult>(IUnitRequest query, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public async Task<TResult> HandleQueryAsync<TResult>(IUnitRequest request, CancellationToken cancel = default) where TResult : class
        {
            TResult result = default(TResult);
            using (var connection = DataProvider.CreateConnection(ConnectionString))
            {
                connection.Open();
                using (var dbCmd = connection.CreateCommand())
                {
                    dbCmd.CommandText = request.GetCommand(RequestOptions.JsonAuto);
                    var json = await ReadAsync(dbCmd, cancel);
                    result = JsonConvert.DeserializeObject<TResult>(json);
                }
                connection.Close();
            }
            return result;
        }

        private static async Task<string> ReadAsync(DbCommand cmd, CancellationToken cancel)
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

        public async Task<IMetaCollection> HandleReadAsync(IUnitRequest cmd, CancellationToken cancel = default)
        {
            var array = await HandleRawAsync(cmd.GetCommand(), cancel);
            return new MetaCollection(array);
        }

        public async Task<JObject> HandleScalarAsync(IUnitRequest cmd, CancellationToken cancel = default)
        {
            StringBuilder result = new StringBuilder();

            await HandleAsync(cmd.GetCommand(), result, cancel);
            if (result.Length > 0)
                return JObject.Parse(result.ToString());
            else
                return default;
        }

        public async Task<ICollection<TResult>> HandleRequestAsync<TResult>(IUnitRequest cmd, CancellationToken cancel = default) where TResult : class
        {
            return await HandleQueryAsync<Unit<TResult>.Collection>(cmd, cancel);
        }

        protected virtual async Task<JArray> HandleRawAsync(string sql, CancellationToken cancellationToken = default)
        {
            JArray result = default;
            using (var conn = DataProvider.CreateConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    var json = await ReadAsync(cmd, cancellationToken);
                    result = JArray.Parse(json);
                }
                conn.Close();
            }
            return result;

        }

        protected virtual async Task HandleAsync(string sql, StringBuilder result, CancellationToken cancellationToken = default)
        {
            using (var connection = DataProvider.CreateConnection(ConnectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                    if (!reader.HasRows)
                        reader.Close();
                    else
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            result.Append(reader.GetString(0));
                            //object[] arr = new object[reader.FieldCount];
                            //reader.GetValues(arr);
                            //  result.AppendJoin(',', arr);
                        }
                        reader.Close();
                    }
                }

            }
            //return JsonConvert.DeserializeObject<TEntity>(result.ToString());
        }

        public async Task<IMetaCollection<TUnit>> ReadAsync<TUnit>(Expression<Predicate<TUnit>> criteria = default, CancellationToken cancel = default) where TUnit : UnitBase
        {
            var request = provider.ReadRequest<TUnit>(criteria);

            var items = await HandleRawAsync(request.GetCommand(), cancel);

            return new MetaCollection<TUnit>(items);
        }

        public async Task<TResult> HandleRawAsync<TResult>(string request, CancellationToken cancel = default) where TResult : class
        {
            var command =  provider.FromRaw<TResult>(request);
            command.RequestOptions = RequestOptions.JsonAuto | RequestOptions.Scalar;
            var item = await HandleScalarAsync(command, cancel);
            var result = item?.ToObject<TResult>();
            if (result is UnitBase unit)
                unit.InitializeMetaData();
            return result;
        }

        public async Task<IMetaCollection<TUnit>> HandleQueryAsync<TUnit>(IUnitRequest<TUnit> query, CancellationToken cancel = default) where TUnit : UnitBase
        {
            var items = await HandleRawAsync(query.GetCommand(), cancel);

            return new MetaCollection<TUnit>(items);
        }
    }
}

