using BotService;
using MissBot.Abstractions.DataAccess;
using MissBot.DataAccess;
using MissBot.Handlers;
using MissDataMaiden.Commands;
using MissDataMaiden.DataAccess;

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
                    .UseMessageHandler<MessageHandler>()
                    .UseInlineAnswerHandler<MissDataAnswerHandler>()
                    .UseCallbackDispatcher<MissDataCallBackDispatcher>()
                    .UseInlineHandler<SearchDataBaseHandler>()                    
                    .AddRepository<IJsonRepository, JsonRepository>()
                    .AddHandler<AddCommandHadler>()
                    .AddCommand<Disk, DiskCommandHandler>()
                    .AddCommand<Info, InfoCommandHadler>()
                    .AddCommand<List, ListCommandHadler>() 
                    .AddAction<DBInfo, DdActionHandler>()
                    .AddAction<DBRestore, DdActionHandler>()
                    .AddAction<DBDelete, DdActionHandler>();

            botHost.Start();
        }
    }
}
