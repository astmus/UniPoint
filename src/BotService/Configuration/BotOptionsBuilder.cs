using System.Runtime.CompilerServices;
using BotService.Interfaces;
using Telegram.Bot.Types.Enums;

namespace BotService.Configuration
{

    internal class BotOptionsBuilder : IBotOptionsBuilder
    {
        const string BaseTelegramUrl = "https://api.telegram.org";
        public BotConnectionOptions Options;
        public IEnumerable<UpdateType> updates = Enumerable.Empty<UpdateType>();
        List<Action> updItems;

        public BotOptionsBuilder(BotConnectionOptions options)
        {
            updItems = new List<Action>();
            With = action
                =>
            { updItems.Add(action); return this; };
            Options = options;
            updates = updates.Append(UpdateType.Message);
        }

        public IBotOptionsBuilder SetToken(string token, string baseUrl = default, bool useTestEnvironment = false)
            => With(() => Options.Token = token);

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
        public IBotOptionsBuilder SetTimeout(TimeSpan timeout)
            => With(() => Options.Timeout = timeout);

        public IBotConnectionOptions Build()
        {
            foreach (var action in updItems)
                action();
            Options.AllowedUpdates = updates.ToArray();
            return Options;
        }

        public IBotOptionsBuilder SetExceptionHandler(Func<Exception, CancellationToken, Task> handlerFactory)
        => With(() => Options.ConnectionErrorHandler = handlerFactory);

        public IBotOptionsBuilder UseCustomUpdateHandler()
            => With(() => Options.UseCustomParser = true);
    }
}
