using System;
using System.Text.Json.Nodes;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissCore.Data;

namespace MissCore.Bot
{
    [JsonObject]
    public record ContentUnit<TEntity> : Unit<TEntity>, IContentUnit<TEntity>
    {
        Collection content;
        public Collection Content
        {
            get =>
                content ?? (content = new Collection());
            set => content = value;
        }
        public override string EntityKey
            => Key;

        IEnumerable<TEntity> IContentUnit<TEntity>.Content
            => this.Content;

        //public static readonly TEntity Default = Activator.CreateInstance<TEntity>();
        public record Empty : Unit<TEntity>
        {
            public override string EntityKey
                => Unit<TEntity>.Key;
        }
    }


}
