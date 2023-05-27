using BotService;
using MissBot.Abstractions.DataAccess;
using MissBot.DataAccess;
using MissBot.Handlers;
using MissCore.Bot;
using MissDataMaiden.Commands;
using MissDataMaiden.DataAccess;
using MissDataMaiden.Entities;

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
                    .UseInlineAnswerHandler<MissDataAnswerHandler>()
                    .UseCallbackDispatcher<MissDataCallBackDispatcher>()
                    .UseInlineHandler<SearchDataBaseHandler>()
                    .UseMessageHandler<MessageHandler>()
                    .AddRepository<IJsonRepository, JsonRepository>()
                    .AddCustomCommandHandler<CustomCommandCreateHandler>()
                    .AddUnitActionHandler()
                    .AddHandler<AddCommandHadler>()     
                    
                    .AddCommand<Disk, DiskCommandHandler>()
                    //.AddCommand<Info, InfoCommandHadler>()
                    .AddCommand<List, ListCommandHadler>();

            botHost.Start();
        }
    }
}
