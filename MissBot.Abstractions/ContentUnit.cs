using System.Text.Json.Nodes;


namespace MissBot.Abstractions
{
    [JsonObject]
    public record ContentUnit<TEntity> : Unit
    {
        Union<TEntity> content;
        public Union<TEntity> Content
        {
            get =>
                content ?? (content = new Union<TEntity>());
            set => content = value;
        }

        public static readonly Empty Default = new Empty("0");
        public record Empty(object Id, string Text = "Empty", string Title = "Not found") : Unit()
        {
            public TEntity[] Content { get; set; } = { Unit<TEntity>.Sample };
        }
    }
}
