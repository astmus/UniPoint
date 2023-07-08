using BotService.Internal;
using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.Bot;
using MissBot.Entities;
using MissBot.Entities.API;
using MissBot.Entities.Enums;
using MissCore.Data;

namespace BotService.Connection.Extensions
{
	public static class ConnectionExtensions
	{
		public static async Task SyncCommandsAsync(this IBotConnection botClient, IEnumerable<IBotCommand> commands, BotCommandScope scope = default, string languageCode = default,
				CancellationToken cancellationToken = default
			)
		{
			//var s = JsonConvert.SerializeObject(new  SetMyCommandsRequest(commands.Select(s => new BotCommand() { Command = s.CommandAction, Description = s.Description })));
			await botClient.HandleQueryAsync(
					 request: new SetMyCommandsRequest(commands.Select(s => new BotCommand() { Action = s.Command, Description = s.Description }))
					 {
						 Scope = scope,
						 LanguageCode = languageCode
					 },
					 cancellationToken
				 )
				 .ConfigureAwait(false);
		}


		/// <summary>
		/// Will attempt to throw the last update using offset set to -1.
		/// </summary>
		/// <param name="botClient"></param>
		/// <param name="cancellationToken"></param>
		/// <returns>
		/// Update ID of the last <see cref="Update"/> increased by 1 if there were any
		/// </returns>
		internal static async Task<uint> ThrowOutPendingUpdatesAsync(
			this IBotConnection botClient,
			CancellationToken cancellationToken = default)
		{
			var request = new GetUpdatesRequest<Update>
			{
				Limit = 1,
				Offset = 0,
				Timeout = 0,
				AllowedUpdates = Array.Empty<UpdateType>(),
			};
			var updates = await botClient.HandleQueryAsync(request: request, cancellationToken: cancellationToken)
				.ConfigureAwait(false);

#if NET6_0_OR_GREATER
			if (updates.Length > 0) { return updates[^1].Id + 1; }
#else
        if (updates.Length > 0) { return updates[updates.Length - 1].Id + 1; }
#endif
			return 0;
		}
	}
}
