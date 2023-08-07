using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Bot;
using MissBot.Abstractions.Handlers;
using MissBot.Entities;
using MissBot.Entities.Abstractions;
using MissBot.Extensions;

using MissCore.Bot;
using MissCore.Data;
using MissCore.Extensions;
using MissBot.Identity;

namespace MissCore.Handlers
{
	public record UnitActionHandler<TUnit>(IHandleContext Context) : IAsyncUnitActionHanlder<TUnit> where TUnit : BaseUnit
	{
		public void HandleUnitAction(IUnitAction<TUnit> action)
		{
			using (CancellationTokenSource src = new CancellationTokenSource())
			{
				//Response.Write("action");
				Task.WaitAll(HandleUnitActionAsync(action, src.Token));
			}
		}

		public async Task HandleUnitActionAsync(IUnitAction<TUnit> action, CancellationToken cancel = default)
		{
			var Response = Context.Response<TUnit>();
			for (int i = 0; i < 20; i++)
			{
				Response.AddUnit(DataUnit<TUnit>.Init(action));
				await Task.Delay(500);
				await Response.Commit(cancel);
			}
		}
	}
}
