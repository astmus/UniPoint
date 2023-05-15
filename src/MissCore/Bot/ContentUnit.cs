using System.Text.Json.Nodes;
using MissCore.Collections;

namespace MissCore.Bot
{
    [JsonObject]
    public record ContentUnit<TEntity> : Unit
    {
        Unit<TEntity>.Collection content;
        public Unit<TEntity>.Collection Content
        {
            get =>
                content ?? (content = new Unit<TEntity>.Collection());
            set => content = value;
        }

        public static readonly Empty Default = new Empty("0");
        public record Empty(object Id, string Text = "Empty", string Title = "Not found") : Unit()
        {
            public TEntity[] Content { get; set; } = { Unit<TEntity>.Sample };
        }
    }


}
