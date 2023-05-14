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
                    //.UseCommndFromAttributes()
                    // .UseMediator()
                    // .UseLogging()
                    .UseCommandsRepository<BotCommandsRepository>()
                    .UseCommandDispatcher<MissDataCommandDispatcher>()
                    .UseInlineAnswerHandler<MissDataAnswerHandler>()
                    .UseCallbackDispatcher<MissDataCallBackDispatcher>()
                    .UseInlineHandler<SearchDataBaseHandler>()
                    .AddRepository<IBotRepository, BotRepository>()
                    .AddRepository<IJsonRepository, JsonRepository>()
                    .AddCommand<Disk, DiskCommandHandler, DiskResponse>()
                    .AddCommand<Info, InfoCommandHadler>()
                    .AddCommand<List, ListCommandHadler>()
                    .AddAction<DBInfo, DdActionHandler>()
                    .AddAction<DBRestore, DdActionHandler>()
                    .AddAction<DBDelete, DdActionHandler>();


            //botHost.AddBot<CoreBot.HostedCore.>();
            botHost.Start();
            //  .RunBot< BotUpdate>();
        }
    }
}
