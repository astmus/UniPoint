using MissBot.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Requests;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using MissBot.Common;
using MissBot.Extensions.Response;

namespace MissBot.Common
{

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record Response<T> : ResponseMessage<T>, IResponse<T>
    {
        BotClientDelegate Client;
        Message message;
        Chat channel;
        public override ChatId ChatId
            => channel.Id;
        
        public async Task Commit(CancellationToken cancel)
        {
            message = Result = await Client().SendQueryRequestAsync(this, cancel);
        }
        public void Init(ICommonUpdate update, BotClientDelegate sender, T data = default)
        {
            channel = update.Chat;
            message = update.Message;
            Client = sender;
        }
        public async Task<IResponseChannel> InitAsync(T data, ICommonUpdate update, BotClientDelegate sender)
        {
            Init(update, sender);
            return await Client().SendQueryRequestAsync(new GetChannelQuery<T>(channel.Id));
        }        
        public override void Write<TUnitData>(TUnitData unit)
        {            
            WriteUnit(unit);            
        }

        public void WriteResult<TUnitData>(TUnitData units) where TUnitData : BotUnion
        {
            foreach (var unit in units)
                Write(unit);
        }
        public void Write<TUnitData>(IEnumerable<TUnitData> units) where TUnitData : Unit<T>
        {
            foreach (var unit in units)
                Write(unit);
        }

        protected virtual Response<T> WriteUnit(BotUnit unit)
        {
            Text += unit.ToString().AsSection("Section");
            return this;
        }
    }

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class GetChannelQuery<T> : RequestBase<ResponseChannel<T>>, IChatTargetable
    {
        /// <inheritdoc />
        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(ChatIdConverter))]
        public ChatId ChatId { get; }

        /// <summary>
        /// Initializes a new request with chatId
        /// </summary>
        /// <param name="chatId">
        /// Unique identifier for the target chat or username of the target supergroup or channel
        /// (in the format <c>@channelusername</c>)
        /// </param>
        public GetChannelQuery(ChatId chatId)
            : base("getChat")
        {
            ChatId = chatId;
        }
    }

    internal class ChatIdConverter : JsonConverter<ChatId?>
    {
        public override void WriteJson(JsonWriter writer, ChatId? value, JsonSerializer serializer)
        {
            switch (value)
            {
                case { Username: { } username }:
                    writer.WriteValue(username);
                    break;
                case { Identifier: { } identifier }:
                    writer.WriteValue(identifier);
                    break;
                case null:
                    writer.WriteNull();
                    break;
                default:
                    throw new JsonSerializationException();
            }
        }

        public override ChatId? ReadJson(
            JsonReader reader,
            Type objectType,
            ChatId? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            var value = JToken.ReadFrom(reader).Value<string>();
            return value is null ? null : new(value);
        }
    }
}


