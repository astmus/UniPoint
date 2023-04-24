using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.DataAccess.Sql;
using MissCore.Entities;

namespace MissCore.Bot
{
    public record Search : BotUnit
    {

    }
    public record Filter(int skip, int take, string predicat);
    public record Search<TEntity> : SQL<TEntity>
    {
        public Filter Filter { get; protected set; } = new Filter(0, 15, "");
        public override SQLCommand Command
            => string.Format(Command, Filter.skip, Filter.take, Filter.predicat);

        public SQL<TEntity> ToQuery(Filter filter)
        {
            Filter = filter;
            return this;
        }
    }
}
