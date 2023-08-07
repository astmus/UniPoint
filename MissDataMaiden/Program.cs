using BotService;
using MissBot.Abstractions.Bot;
using MissBot.Abstractions.DataAccess;
using MissBot.DataAccess;
using MissCore.DataAccess;
using MissCore.Handlers;
using MissDataMaiden.Commands;
using MissDataMaiden.DataAccess;
using MissDataMaiden.Entities;

namespace MissDataMaiden
{

	public class Program
	{
		public static void Main(string[] args)
		{
			var botHost = BotHost.CreateDefault(args);
			botHost.AddBot<MissDataMaid, MissDataMaidConfigurator>()
					.Use<ExceptionHandler>()
					.UseCommandsRepository<BotCommandsRepository>()
					.UseCommandDispatcher<MissDataCommandDispatcher>()
					.UseInlineAnswerHandler<MissDataAnswerHandler>()
					.UseCallbackDispatcher<MissDataCallBackDispatcher>()
					.UseInlineHandler<DataBase, SearchDataBaseHandler>()
					.UseMessageHandler<MessageHandler>()
					.AddGenericRepository<IJsonRepository, JsonRepository>()
					.AddRepository<DataBase, GenericRepository<DataBase>>()
					.AddUnit<DataBase, UnitActionHandler<DataBase>>()
					.AddInputParametersHandler()
					.AddCustomCommandCreator<AddCommandHadler>()
					.AddCommand<Disk, DiskCommandHandler>()
					//.AddCommand<Info, InfoCommandHadler>()
					.AddCommand<List, ListCommandHadler>();

			botHost.Start();
		}
	}
}
