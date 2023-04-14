using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using Telegram.Bot.Types;

namespace MissBot.Abstractions
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract class BaseBot : IBot
    {
        private readonly IRepository<BotCommand> commandsRepository;

        public BaseBot()
        {
        }
        public BaseBot(IRepository<BotCommand> commandsRepository)
        {
            this.commandsRepository = commandsRepository;
        }

        public abstract Func<ICommonUpdate, string> ScopePredicate { get; }
        public abstract void ConfigureConnection(IBotConnectionOptionsBuilder connectionBuilder);
        public abstract void ConfigureOptions(IBotOptionsBuilder botBuilder);

        public async Task<bool> SyncCommands(IBotConnection connection)
        {
            var commands = await commandsRepository.GetAllAsync();            
            return await connection.SyncCommandsAsync(commands);
        }

        #region DTO
        [JsonProperty(Required = Required.Always)]
        public long Id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string FirstName { get; set; } = default!;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? LastName { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Username { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? LanguageCode { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? IsPremium { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? AddedToAttachmentMenu { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanJoinGroups { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? CanReadAllGroupMessages { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? SupportsInlineQueries { get; set; }

        /// <inheritdoc/>
        public override string ToString() =>
            $"{(Username is null ? $"{FirstName}{LastName?.Insert(0, " ")}" : $"@{Username}")} ({Id})";

        #endregion

    }
    internal static class BotExtensoin
    {
        #region Extensions
        internal static async Task<bool> SyncCommandsAsync(this IBotConnection botClient,  IEnumerable<BotCommand> commands, BotCommandScope scope = default, string languageCode = default,
                 CancellationToken cancellationToken = default)       
        => await botClient.MakeRequestAsync(
                     request: new Telegram.Bot.Requests.SetMyCommandsRequest(commands)
                     {
                         Scope = scope,
                         LanguageCode = languageCode
                     },
                     cancellationToken
                 )
                 .ConfigureAwait(false);
        
        #endregion
    }
}
