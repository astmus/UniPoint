using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Bot;
using MissBot.Abstractions.Handlers;

using MissCore;
using MissCore.Bot;
using MissCore.Data;
using MissCore.Data.Collections;
using MissCore.DataAccess;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using MissBot.Identity;

namespace MissDataMaiden.Commands
{
	[JsonObject]
	public record Disk : BotUnitCommand
	{
	}


	public record DiskCommand : BotCommand<Disk>
	{
	}

	public class DiskCommandHandler : BotCommandHandler<Disk>
	{
		private readonly IJsonRepository repository;

		public DiskCommandHandler(IJsonRepository repository)
		{
			this.repository = repository;
		}

		public async override Task HandleCommandAsync(Disk command, CancellationToken cancel = default)
		{
			//var response = Context.Response<Disk>();


			Response.SetCounter(Id<Disk>.Instance.Sequence());



			var metaCollection = await repository.RawAsync<GenericUnit>(command.Template);

			//var items = metaCollection;
			//response.Write(metaCollection.Enumarate<DataUnit<Disk>>());
			foreach (var item in metaCollection.Content)
			{
				Response.Write(item);
			}

			await Response.Commit(default);
		}
	}
}
