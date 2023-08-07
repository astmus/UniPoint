using System.Runtime.CompilerServices;
using BotService.Internal;
using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.Bot;
using MissBot.Abstractions.Presentation;
using MissBot.Entities;
using MissBot.Entities.Enums;
using Newtonsoft.Json;
using LinqToDB.Mapping;

namespace BotService.Configuration
{

	internal class BotContextBuilder : IBotOptionsBuilder, IBotConnectionOptionsBuilder
	{
		const string BaseTelegramUrl = "https://api.telegram.org";
		public BotConnectionOptions Options;
		public IEnumerable<UpdateType> updates = Enumerable.Empty<UpdateType>();
		List<Action> updItems;
		MappingSchema myFluentMappings = new MappingSchema();
		FluentMappingBuilder builder;
		public BotContextBuilder(IEnumerable<JsonConverter> converters)
		{
			builder = new FluentMappingBuilder(myFluentMappings);

			updItems = new List<Action>();

			With = action
				=>
			{ updItems.Add(action); return this; };
			WithOptions = action
				=>
			{ action(); return this; };

			Options = new();

			foreach (var con in converters)
				Options.SerializeSettings.Converters.Add(con);

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
		Func<Action, IBotConnectionOptionsBuilder> WithOptions;

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
			foreach (var action in updItems)
				action();
			return Options;
		}

		public IBotConnectionOptionsBuilder SetTimeout(TimeSpan timeout)
			=> WithOptions(() => Options.Timeout = timeout);

		public IBotConnectionOptionsBuilder SetExceptionHandler(Func<Exception, CancellationToken, Task> handlerFactory)
			=> WithOptions(() => Options.ConnectionErrorHandler = handlerFactory);

		public IBotConnectionOptionsBuilder UseCustomUpdateHandler()
			=> WithOptions(() => Options.UseCustomParser = true);

		public IBotUnitBuilder AddResponseUnit<TUnit>() where TUnit : BaseBotUnit
		{
			throw new NotImplementedException();
		}

		public IBotUnitBuilder Apply<TDecorator>() where TDecorator : UnitItemSerializeDecorator
		{
			throw new NotImplementedException();
		}
	}
}
