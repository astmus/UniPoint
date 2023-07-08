using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissBot.Entities.Enums;
using MissCore.Data;

namespace MissDataMaiden
{

	//[HasBotCommand(Name = nameof(List), CmdType = typeof(List), Description = "List of data bases with info")]
	//[HasBotCommand(Name = nameof(Info), CmdType = typeof(Info), Description = "Inforamtion about current server state")]
	//[HasBotCommand(Name = nameof(Disk), CmdType = typeof(Disk), Description = "Disk space information")]
	public class MissDataMaid : BaseBot
	{
		private readonly ILogger<MissDataMaid> _logger;
		private ILogger<MissDataMaid> log;
		private readonly IBotContext<MissDataMaid> botContext;
		public override Func<IUnitUpdate, string> ScopePredicate
			 => (update) => update switch
			 {
				 UnitUpdate upd when upd.Type is UpdateType.InlineQuery => $"{upd.Chat.Id}",
				 //UnitUpdate upd when upd.Type is UpdateType.CallbackQuery => $"{upd.CallbackQuery.Id}",
				 UnitUpdate upd => $"{nameof(upd.Chat)}: {upd.Chat.Id}",
				 _ => null
			 };

		public MissDataMaid(ILogger<MissDataMaid> logger, IBotContext<MissDataMaid> context) : base(context)
		{
			log = logger;
			botContext = context;
		}

		protected override void LoadBotInfrastructure()
		{
			botContext.LoadBotInfrastructure();
		}
		public static Task ErrorHandler(Exception error, CancellationToken cancel)
		{
			Console.WriteLine(error);
			return Task.CompletedTask;
		}
	}

	public class MissDataMaidConfigurator : BaseBot.Configurator
	{
		public override void ConfigureOptions(IBotOptionsBuilder botBuilder)
			=> botBuilder
						.ReceiveCallBacks()
						.ReceiveInlineQueries()
						.ReceiveInlineResult()
						.TrackMessgeChanges();

		public override void ConfigureConnection(IBotConnectionOptionsBuilder connectionOptions)
			=> connectionOptions
					.SetToken(Environment.GetEnvironmentVariable("AliseBot", EnvironmentVariableTarget.User))
					.SetTimeout(TimeSpan.FromMinutes(2))
					.SetExceptionHandler(MissDataMaid.ErrorHandler);
	}
}
