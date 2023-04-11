using BotService;
using BotService.Common;
using BotService.Configuration;
using MissBot.Handlers;
using MissDataMaiden.Commands;
using MissBot.Extensions;
namespace MissDataMaiden
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var botHost = BotHost.CreateDefault(args);
            botHost.AddBot<MissDataMaid>()
                .UseCommndFromAttributes()
                    .UseMediator()
                    .UseLogging()                    
                    .Use<ExceptionHandler>()                                        
                    .UseCommandDispatcher<MissDataCommandHandler>()
                    .UseCallbackDispatcher<MissDataCallBackDispatcher>()
                    .UseInlineHandler<ListDiskInlineHandler>()
                    .AddCommand<Disk, DiskCommandHandler, DiskResponse>()
                    .AddCommand<Info, InfoCommandHadler>()
                    .AddCommand<List, ListCommandHadler>()
                    .AddAction<DBInfo, DdActionHandler>()
                    .AddAction<DBRestore, DdActionHandler>()
                    .AddAction<DBDelete, DdActionHandler>();


            botHost.AddBot<MissChannel>();
            botHost.Start();
            //  .RunBot< BotUpdate>();
        }
    }
}
