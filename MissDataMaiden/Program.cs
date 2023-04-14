using BotService;
using BotService.Common;
using BotService.Configuration;
using MissBot.Handlers;
using MissDataMaiden.Commands;
using MissBot.Extensions;
using MissDataMaiden.DataAccess;

namespace MissDataMaiden
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var botHost = BotHost.CreateDefault(args);
            botHost.AddBot<MissDataMaid>()
                    .Use<ExceptionHandler>()                                        
                //.UseCommndFromAttributes()
                    .UseMediator()
                    .UseLogging()                    
                    .UseCommandsRepository<BotCommandsRepository>()
                    .UseCommandDispatcher<MissDataCommandDispatcher>()
                    .UseCallbackDispatcher<MissDataCallBackDispatcher>()
                    .UseInlineHandler<ListDiskInlineHandler>()
                    .AddCommand<Disk, DiskCommandHandler, DiskResponse>()
                    .AddCommand<Info, InfoCommandHadler>()
                    .AddCommand<List, ListCommandHadler>()
                    .AddAction<DBInfo, DdActionHandler>()
                    .AddAction<DBRestore, DdActionHandler>()
                    .AddAction<DBDelete, DdActionHandler>();

                    
          //  botHost.AddBot<MissChannel>();
            botHost.Start();
            //  .RunBot< BotUpdate>();
        }
    }
}
