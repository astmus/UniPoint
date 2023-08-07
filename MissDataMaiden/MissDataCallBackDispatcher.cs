using MissBot.Abstractions;
using MissBot.Abstractions.Bot;
using MissBot.Abstractions.DataAccess;
using MissBot.Entities.Abstractions;
using MissBot.Entities.Query;
using MissBot.Identity;
using MissCore.Data.Collections;
using MissCore.Handlers;
using MissDataMaiden.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using MissBot.Extensions;
using System.Text.RegularExpressions;
using LinqToDB;

namespace MissDataMaiden
{

	internal class MissDataCallBackDispatcher : CallbackQueryHandler
	{
		IJsonRepository repo;
		public MissDataCallBackDispatcher(IResponseNotification notifier, IJsonRepository repository) : base(notifier)
			=> repo = repository;

		IInteraction<DataBase> response;
		protected override async Task HandleAsync(string command, string unit, string id, CallbackQuery query, CancellationToken cancel = default)
		{
			switch (unit)
			{
				case nameof(DataBase):
					var unitAction = Context.Set(await Context.Bot.GetUnitActionAsync<DataBase>(command));
					unitAction.Id = Id<DataBase>.Create(id);
					HandleUnitAction(unitAction);
					break;
			}
			//response = Context.BotServices.InteractionOf<DataBase>();
			//var u = Context.Bot.GetBotUnitAsync<DataBase>();
			//	u.Template
			//var source = Context.Bot.GetQueryUnit<DataBase>();
			//dbRepo.
			//source.SourceContext = userDataContext.FromSql<DataBase>;
			//var db = source.Get(id);
			//var dbs = await repo.HandleQueryAsync<DataBase>(source, cancel); //<DataBase>(source.Get()).FirstOrDefaultAsync(d => d.Id == id);
			//unitAction.SetUnitContext(dbs);


			//u.Id = Id<DataBase>.Create(id); 
			//var table = Context.Bot.GetRepository<DataBase>(u.Template, id);
			//Context.Bot.SearchResults<DataBase>(null);
			//var d = table.FirstOrDefault();
			//ask.Run(DataBaseDelete, d);
			///0/var tl = d.ToList();

			Context.IsHandled = false;
			//var handler = Context.GetBotService<InputParametersHandler>();
			//await handler.HandleAsync(ParametersEntered, unitAction, Context, cancel);

			//Context.SetNextHandler(handler.AsyncDelegate);
		}


		async Task ParametersEntered(FormattableUnitBase result, IHandleContext context)
		{
			//await response.CompleteInteraction(result).Commit();

			var repository = Context.GetBotService<IJsonRepository>();

			var unitRes = await repository.RawAsync<GenericUnit>(result.Format, default, result.ToArray());
			if (unitRes != null)
				await response.CompleteInteraction(unitRes.Content).Commit();
			else
				await response.CompleteInteraction("Not found").Commit();
			Context.IsHandled = true;
		}
	}
}
