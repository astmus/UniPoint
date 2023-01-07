using BotService;
using BotService.Configuration;
using MissBot.Handlers;
using MissDataMaiden.Commands;

namespace MissDataMaiden
{

    public class Program
    {
        public static void Main(string[] args)
        {
            BotHost.CreateDefault(new BotStartupConfig(), args)
                        .AddCommndFromAttributes<BotWorker>()
                        .Use<ExceptionHandler>()
                        .Use<MediatorMiddleware>()
                        .Use<CallbackQueryHandler>()
                        .Use<Disk, DiskCommandHandler>()
                        .Use<Info, InfoCommandHadler>()
                        .Use<List, ListCommandHnadler>()
                        .BuildHostService()
                        .RunBot();
        }
    }
}
