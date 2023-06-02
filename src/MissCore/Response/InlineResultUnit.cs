
using LinqToDB.Mapping;
using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Entities;
using MissBot.Entities.Query;
using MissBot.Entities.Results.Inline;
using MissCore.Data.Collections;

namespace MissCore.Response
{
    //[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    //public record InlineResultUnit<TEntity> : InlineResultUnit, IUnit<TEntity>
    //{
    //    private const  string empty = "Empty";
    //    public  string this[string path]
    //        => empty;
    //}
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy), ItemNullValueHandling = NullValueHandling.Ignore)]
    [Table("##SearchResults")]
    public record InlineResultUnit<T> : ResultUnit<T>, IUnit<T>
    {
        [JsonProperty]
        public InlineQueryResultType Type { get; set; } = InlineQueryResultType.Article;

        [JsonProperty("input_message_content")]         
        public override InlineContent<T> Content { get; set; } = new InlineContent<T>();

        [Column]
        [JsonProperty]
        public override string Id { get; set; }

        [Column]
        [JsonProperty]
        public override string Title { get; set; }

        [Column]
        [JsonProperty]
        public override string Description { get; set; }

        [JsonProperty("reply_markup", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public override IActionsSet Actions { get; set; }

        [Column("Entity")]
        [JsonProperty("Entity")]
        public override string EntityKey { get; set; }


        [Column("Unit")]
        [JsonProperty("Unit")]
        public override string UnitKey { get; set; }
        public override void InitializeMetaData()
            => Meta ??= MetaData.Parse(Content);
        
    }
}
