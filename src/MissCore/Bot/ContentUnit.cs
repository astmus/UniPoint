using System.Text.Json.Nodes;
using MissBot.Abstractions.Entities;
using MissCore.Collections;

namespace MissCore.Bot
{
    [JsonObject]
    public record ContentUnit<TEntity> : Unit<TEntity>
    {
        Collection content;
        public Collection Content
        {
            get =>
                content ?? (content = new Collection());
            set => content = value;
        }
        public override string Entity
            => Key;

        public static readonly Empty Default = new Empty("0");
        public record Empty(object Id, string Text = "Empty", string Title = "Not found") : Unit<TEntity>
        {
            public TEntity[] Content { get; set; }
            public override string Entity
                => Unit<TEntity>.Key;
        }
    }


}
