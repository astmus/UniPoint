using BotService;
using BotService.Common;
using BotService.Configuration;
using MissBot.Handlers;
using MissDataMaiden.Commands;
using MissBot.Extensions;
using MissDataMaiden.DataAccess;
using MissCore.Bot;
using MissBot.DataAccess;
using MissBot.Abstractions.DataAccess;

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
                    .UseMediator()
                    .UseLogging()                    
                    .UseCommandsRepository<BotCommandsRepository>()
                    .UseCommandDispatcher<MissDataCommandDispatcher>()
                    .UseInlineAnswerHandler<MissDataAnswerHandler>()
                    .UseCallbackDispatcher<MissDataCallBackDispatcher>()
                    .UseInlineHandler<ListDiskInlineHandler>()
                    .AddCommand<Disk, DiskCommandHandler, DiskResponse>()
                    .AddCommand<Info, InfoCommandHadler>()
                    .AddCommand<List, ListCommandHadler>()
                    .AddAction<DBInfo, DdActionHandler>()
                    .AddAction<DBRestore, DdActionHandler>()
                    .AddAction<DBDelete, DdActionHandler>()
                    .AddRepository<IJsonRepository, JsonSQLRepository>();

                    
          //botHost.AddBot<CoreBot.HostedCore.>();
            botHost.Start();
            //  .RunBot< BotUpdate>();
        }
    }
}
