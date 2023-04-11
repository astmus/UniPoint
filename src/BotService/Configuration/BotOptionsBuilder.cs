using System.Runtime.CompilerServices;
using MissBot.Abstractions.Configuration;
using Telegram.Bot.Types.Enums;

namespace BotService.Configuration
{

    internal class BotOptionsBuilder : IBotOptionsBuilder, IBotConnectionOptionsBuilder
    {
        const string BaseTelegramUrl = "https://api.telegram.org";
        public BotConnectionOptions Options;
        public IEnumerable<UpdateType> updates = Enumerable.Empty<UpdateType>();
        List<Action> updItems;

        public BotOptionsBuilder()
        {
            updItems = new List<Action>();
            With = action
                =>
            { updItems.Add(action); return this; };
            Options = new();
            updates = updates.Append(UpdateType.Message);
        }

        public IBotConnectionOptionsBuilder SetToken(string token, string baseUrl = default, bool useTestEnvironment = false)
        {
            ParseToken(token);
            return this;
        }

        IBotOptionsBuilder ParseToken(string token, string baseUrl = default, bool useTestEnvironment = false)
        {
            Options.Token = token;
            Options.BaseUrl = baseUrl;
            Options.UseTestEnvironment = useTestEnvironment;

            Options.BotId = GetIdFromToken(token);

            Options.LocalBotServer = baseUrl is not null;
            var effectiveBaseUrl = Options.LocalBotServer
                ? ExtractBaseUrl(baseUrl)
                : BaseTelegramUrl;

            Options.BaseRequestUrl = useTestEnvironment
                ? $"{effectiveBaseUrl}/bot{token}/test"
                : $"{effectiveBaseUrl}/bot{token}";

            Options.BaseFileUrl = useTestEnvironment
                ? $"{effectiveBaseUrl}/file/bot{token}/test"
                : $"{effectiveBaseUrl}/file/bot{token}";
            return this;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static long GetIdFromToken(string token)
            {
                var span = token.AsSpan();
                var index = span.IndexOf(':');

                if (index is < 1 or > 16) { return default; }

                var botIdSpan = span[..index];
                if (!long.TryParse(botIdSpan, out var botId)) { return default; }
                return botId;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static string ExtractBaseUrl(string baseUrl)
        {
            if (baseUrl is null) { throw new ArgumentNullException(paramName: nameof(baseUrl)); }

            if (!Uri.TryCreate(uriString: baseUrl, uriKind: UriKind.Absolute, out var baseUri) || string.IsNullOrEmpty(value: baseUri.Scheme) || string.IsNullOrEmpty(value: baseUri.Authority))
                throw new ArgumentException(message: "Invalid format. A valid base url looks \"http://localhost:8081\" ", paramName: nameof(baseUrl));

            return $"{baseUri.Scheme}://{baseUri.Authority}";
        }
        Func<Action, IBotOptionsBuilder> With;

        public IBotOptionsBuilder ReceiveCallBacks()
            => With(() => updates = updates.Append(UpdateType.CallbackQuery));
        public IBotOptionsBuilder ReceiveInlineQueries()
            => With(() => updates = updates.Append(UpdateType.InlineQuery));
        public IBotOptionsBuilder ReceiveInlineResult()
            => With(() => updates = updates.Append(UpdateType.ChosenInlineResult));
        public IBotOptionsBuilder TrackMessgeChanges()
            => With(() => updates = updates.Append(UpdateType.EditedMessage));
        public IBotConnectionOptions Build()
        {
            foreach (var action in updItems)
                action();
            Options.AllowedUpdates = updates.ToArray();
            return Options;
        }

        public IBotConnectionOptionsBuilder SetTimeout(TimeSpan timeout)
        {
            Options.Timeout = timeout;
            return this;
        }

        public IBotConnectionOptionsBuilder SetExceptionHandler(Func<Exception, CancellationToken, Task> handlerFactory)
        {
            Options.ConnectionErrorHandler = handlerFactory;
            return this;
        }

        public IBotConnectionOptionsBuilder UseCustomUpdateHandler()
        {
            Options.UseCustomParser = true;
            return this;
        }       
    }
}
