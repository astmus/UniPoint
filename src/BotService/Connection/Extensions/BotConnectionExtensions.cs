using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Entities;
using MissCore.Data;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace BotService.Connection.Extensions
{
    public static class ConnectionExtensions
    {           
        public static async Task SyncCommandsAsync(
                this IBotConnection botClient,
                IEnumerable<IBotCommand> commands,
                BotCommandScope scope = default,
                string languageCode = default,
                CancellationToken cancellationToken = default
            )
        {
            //var s = JsonConvert.SerializeObject(new  SetMyCommandsRequest(commands.Select(s => new BotCommand() { Command = s.CommandAction, Description = s.Description })));
            await botClient.HandleQueryAsync(
                     request: new  SetMyCommandsRequest(commands.Select(s => new BotCommand() { Command = s.Command, Description = s.Description }))
                     {
                         Scope = scope,
                         LanguageCode = languageCode
                     },
                     cancellationToken
                 )
                 .ConfigureAwait(false);
        }
    }
}
