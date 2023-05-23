using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataContext;
using BotCommand = MissBot.Abstractions.Entities.BotCommand;
using TG = Telegram.Bot.Types;

namespace MissBot.Abstractions
{

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract class BaseBot : IBot
    {
        public abstract class Configurator
        {
            public abstract void ConfigureConnection(IBotConnectionOptionsBuilder connectionBuilder);
            public abstract void ConfigureOptions(IBotOptionsBuilder botBuilder);
        }
        IServiceScope scope;
        public BaseBot(IBotContext botContext)
        {
            Context = botContext;
        }

        public void Initialize(IServiceScope Scope)
        {
            scope = Scope;
            commandsRepository = BotServices.GetRequiredService<IRepository<BotCommand>>();
            Context.LoadBotInfrastructure();
        }

        protected IRepository<BotCommand> commandsRepository { get; set; }
        private readonly IBotContext Context;
        protected IBotConnection Client
            => BotServices.GetRequiredService<IBotConnection>();
        public IBotServicesProvider BotServices
            => scope.ServiceProvider.GetRequiredService<IBotServicesProvider>();

        public IEnumerable<BotCommand> Commands { get; protected set; }
        public abstract Func<IUnitUpdate, string> ScopePredicate { get; }

        public virtual async Task<bool> SyncCommands()
        {
            Commands ??= await commandsRepository.GetAllAsync();
            return await Client.SyncCommandsAsync(Commands);
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
        internal static async Task<bool> SyncCommandsAsync(this IBotConnection botClient, IEnumerable<BotCommand> commands, TG.BotCommandScope scope = default, string languageCode = default,
                 CancellationToken cancellationToken = default)
        => await botClient.MakeRequestAsync(
                     request: new SetMyCommandsRequest(commands)
                     {
                         Scope = scope,
                         LanguageCode = languageCode
                     },
                     cancellationToken
                 )
                 .ConfigureAwait(false);

        [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        record SetMyCommandsRequest : BaseRequest<bool>
        {

            [JsonProperty(Required = Required.Always)]
            public IEnumerable<BotCommand> Commands { get; }


            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
            public TG.BotCommandScope? Scope { get; set; }


            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
            public string? LanguageCode { get; set; }

            public SetMyCommandsRequest(IEnumerable<BotCommand> commands)
                : base("setMyCommands")
            {
                Commands = commands;
            }
        }

        #endregion
    }

}
