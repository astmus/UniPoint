using BotService;
using BotService.Common;
using BotService.Configuration;
using MissBot.Handlers;
using MissDataMaiden.Commands;

namespace MissDataMaiden
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var botHost = BotHost.CreateDefault(args);
            botHost.AddBot<MissDataMaid>()
                .UseCommndFromAttributes()
                    .Use<ExceptionHandler>()
                    .Use<MediatorMiddleware>()
                    .Use<CallbackQueryHandler>()
                    .Use<Disk, DiskCommandHandler>()
                    .Use<Info, InfoCommandHadler>()
                    .Use<List, ListCommandHnadler>();

            botHost.AddBot<MissChannel>();
            botHost.Start();
            //  .RunBot< BotUpdate>();
        }
    }
}
