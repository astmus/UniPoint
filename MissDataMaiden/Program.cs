using BotService;
using BotService.Configuration;
using MissBot.Handlers;
using MissDataMaiden.Commands;

namespace MissDataMaiden
{
    public class BotUpdate : Update
    {

    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BotHost.CreateDefault(new BotStartupConfig(), args);
            host.AddBot<MissDataMaid>(builder=>builder
                .AddCommndFromAttributes<MissDataMaid>()
                .Use<ExceptionHandler>()
                .Use<MediatorMiddleware>()
                .Use<CallbackQueryHandler>()
                .Use<Disk, DiskCommandHandler>()
                .Use<Info, InfoCommandHadler>()
                .Use<List, ListCommandHnadler>());
            host.Run();

            //  .RunBot< BotUpdate>();
        }
    }
}
