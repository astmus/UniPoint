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
                .UseContextHandler<MissDataMaid.ContextHandler>()
                .UseCommndFromAttributes()
                    .UseMediator()
                    .UseLogging()                    
                    .Use<ExceptionHandler>()                    
                    .Use<CallbackQueryHandler>()
                    .Use<MissDataCommandHandler>()
                    .Use<Disk, DiskCommandHandler>()
                    .Use<Info, InfoCommandHadler>()
                    .Use<List, ListCommandHadler>();


            botHost.AddBot<MissChannel>();
            botHost.Start();
            //  .RunBot< BotUpdate>();
        }
    }
}
