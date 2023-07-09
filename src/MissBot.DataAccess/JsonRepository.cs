using System.Data;
using System.Data.Common;
using System.Text;
using LinqToDB;
using Microsoft.Extensions.Options;
using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Bot;
using MissBot.Abstractions.DataAccess;
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

		public async Task<IContentUnit<TResult>> HandleQueryAsync<TResult>(IUnitRequest request, CancellationToken cancel = default) where TResult : class
		{
			ContentUnit<TResult> result = default;
			using (var connection = DataProvider.CreateConnection(ConnectionString))
			{
				await connection.OpenAsync(cancel);
				using (var cmd = connection.CreateCommand())
				{
					cmd.CommandText = request.GetCommand();

					if (await cmd.ReadAsync(cancel) is StringBuilder json)
						result = JsonConvert.DeserializeObject<ContentUnit<TResult>>(json.ToString());
				}
				await connection.CloseAsync().ConfigureAwait(false);
			}
			return result;
		}
		//public async Task<IMetaCollection> ReadAsync(IUnitRequest cmd, CancellationToken cancel = default)
		//{
		//    var array = await HandleRawAsync(JArray.Parse, cmd.GetCommand(), cancel);
		//    return new MetaCollection(array);
		//}
		protected virtual async Task<TJToken> HandleRawAsync<TJToken>(Func<string, TJToken> parser, string sql, CancellationToken cancel = default) where TJToken : JToken
		{
			TJToken result = default;

			using (var conn = DataProvider.CreateConnection(ConnectionString))
			{
				await conn.OpenAsync(cancel);
				try
				{
					using var cmd = conn.CreateCommand();
					cmd.CommandText = sql;
					if (await cmd.ReadAsync(cancel) is StringBuilder json && json.Length > 0)
						result = parser(json.ToString());
				}
				finally
				{
					await conn.CloseAsync().ConfigureAwait(false);
				}
			}
			return result;
		}
		public async Task<JObject> HandleScalarAsync(IUnitRequest cmd, CancellationToken cancel = default)
		{
			var jobj = await HandleRawAsync(JObject.Parse, cmd.GetCommand(), cancel);
			return jobj;
		}

		public async Task<TResult> HandleScalarAsync<TResult>(IUnitRequest request, CancellationToken cancel = default) where TResult : BaseUnit
		{
			TResult result = default;
			using (var conn = DataProvider.CreateConnection(ConnectionString))
			{
				try
				{
					await conn.OpenAsync(cancel);
					using var cmd = conn.CreateCommand();
					request.Options |= RequestOptions.JsonAuto | RequestOptions.Scalar;
					cmd.CommandText = request.GetCommand();

					cmd.LoadParameters(request.Params);

					if (await cmd.ReadAsync(cancel) is StringBuilder json && json.Length > 0)
					{
						var j = JObject.Parse(json.ToString());
						result = Activator.CreateInstance<TResult>();
						result.SetContext(j);
						//result = JsonConvert.DeserializeObject<TResult>(json.ToString());
					}
				}
				catch (Exception error)
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

		public async Task<TResult> HandleRawAsync<TResult>(string request, CancellationToken cancel = default) where TResult : class
		{
			IUnitRequest command = new UnitRequest<Unit<TResult>>(request);
			command.Options |= RequestOptions.Scalar;
			JObject item = await HandleScalarAsync(command, cancel);
			var result = item?.ToObject<TResult>();

			return result;
		}

		public async Task<IMetaCollection<TUnit>> HandleQueryJsonAsync<TUnit>(IUnitRequest query, CancellationToken cancel = default) where TUnit : class
		{
			//var r = await RawAsync<TUnit>(query.GetCommand(), cancel);
			if (await HandleRawAsync(JArray.Parse, query.GetCommand(), cancel) is JArray items)
				return new MetaCollection<TUnit>(items);
			else
				return MetaCollection<TUnit>.Empty;
		}

		public async Task<IContentUnit<TResult>> RawAsync<TResult>(string request, CancellationToken cancel = default, params KeyValuePair<object, object>[] parameters) where TResult : class
		{
			ContentUnit<TResult> result = null;
			using (var conn = DataProvider.CreateConnection(ConnectionString))
			{
				try
				{
					await conn.OpenAsync(cancel);
					using var cmd = conn.CreateCommand();

					cmd.CommandText = $"{request} {(RequestOptions.JsonPath | RequestOptions.RootContent).Format()}";

					cmd.LoadParameters(parameters);

					if (await cmd.ReadAsync(cancel) is StringBuilder json)
					{
						var token = JToken.Parse(json.ToString());
						var result2 = JsonConvert.DeserializeObject<ContentUnit<TResult>>(json.ToString());
					}

					//var r = this.FromSqlScalar<ContentUnit<TResult>>(FormattableStringFactory.Create(cmd.CommandText));
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
	internal static async Task<StringBuilder> ReadAsync(this DbCommand cmd, CancellationToken cancel)
	{
		using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess, cancel);
		if (await reader.ReadAsync(cancel))
		{
			StringBuilder sBuilder = new StringBuilder();
			sBuilder.Append(await reader.GetTextReader(0).ReadToEndAsync(cancel));

			while (await reader.ReadAsync(cancel))
				sBuilder.Append(await reader.GetTextReader(0).ReadToEndAsync(cancel));

			reader.Close();
			return sBuilder;
		}
		return default;
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
	internal static void LoadParameters(this DbCommand cmd, IEnumerable<IUnitParameter> parameters)
	{
		foreach (var arg in parameters)
		{
			var p = cmd.CreateParameter();
			p.ParameterName = "@" + arg.Name;
			p.Value = arg.Value;
			cmd.Parameters.Add(p);
		}
	}
}
