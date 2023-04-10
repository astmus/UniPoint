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
                    .UseCommandHandler<MissDataCommandHandler>()
                    .Use<CallbackQueryHandler>()
                    //.Use<MissDataCommandHandler>()
                    .Use<Disk, DiskCommandHandler, DiskResponse>()
                    .Use<Info, InfoCommandHadler>()
                    .Use<List, ListCommandHadler>();


            botHost.AddBot<MissChannel>();
            botHost.Start();
            //  .RunBot< BotUpdate>();
        }
    }
}
