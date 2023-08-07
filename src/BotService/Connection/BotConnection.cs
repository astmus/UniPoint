using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Entities;

namespace BotService.Connection
{
	/// <summary>
	/// A client to use the Telegram Bot API
	/// </summary>
	public class BotConnection : BaseBotConnection, IBotConnection, IBotClient
	{
		public BotConnection(IBotConnectionOptions options = null, HttpClient httpClient = null) : base(httpClient: httpClient)
		{
			Options = options;
		}

		public override IBotConnectionOptions Options { get; set; }

		uint IBotConnection.Timeout
			=> Convert.ToUInt32(Options.Timeout.TotalSeconds);


		public async Task<TBot> GetBotAsync<TBot>(CancellationToken cancellationToken = default) where TBot : BaseBot
		{
			var info = await HandleQueryAsync<TBot>(request: new BaseParameterlessRequest<TBot>("getMe"), cancellationToken: cancellationToken)
						.ConfigureAwait(false);
			return info;
		}

		public async Task SendCommandAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IBotRequest
		{
			await MakeRequestAsync(request, cancellationToken: cancellationToken)
						.ConfigureAwait(false);
		}

		public async Task<TResponse> SendQueryRequestAsync<TResponse>(IBotRequest<TResponse> request, CancellationToken cancellationToken = default)
		{
			return await HandleQueryAsync<TResponse>(request, cancellationToken: cancellationToken)
						 .ConfigureAwait(false);
		}
	}
}
