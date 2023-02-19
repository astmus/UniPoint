using MissBot.Abstractions;
using MissBot.Commands;
using MissBot.Extensions;
using MissCore.Abstractions;
using Telegram.Bot.Types;

namespace BotService.Connection.Extensions
{
    public static class ConnectionExtensions
    {
        //public static async Task SyncCommandsAsync<TBot>(
        //        this IBotClient<TBot> botClient,
        //        IEnumerable<IBotCommandInfo> commands,
        //        BotCommandScope scope = default,
        //        string languageCode = default,
        //        CancellationToken cancellationToken = default
        //    ) where TBot:IBot =>
        //        await botClient.ThrowIfNull(nameof(botClient))
        //            .MakeRequestAsync(
        //                request: new SetMyCommandsRequest(commands)
        //                {
        //                    Scope = scope,
        //                    LanguageCode = languageCode
        //                },
        //                cancellationToken
        //            )
        //            .ConfigureAwait(false);
        public static async Task SyncCommandsAsync(
                this IBotConnection botClient,
                IEnumerable<IBotCommandInfo> commands,
                BotCommandScope scope = default,
                string languageCode = default,
                CancellationToken cancellationToken = default
            ) =>
                await botClient.MakeRequestAsync(
                        request: new Telegram.Bot.Requests.SetMyCommandsRequest(commands.Select(s=> new BotCommand() { Command = s.Command, Description = s.Description }))
                        {
                            Scope = scope,
                            LanguageCode = languageCode
                        },
                        cancellationToken
                    )
                    .ConfigureAwait(false);
    }
}
