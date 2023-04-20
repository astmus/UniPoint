using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.DataAccess.Sql;
using MissCore.Entities;

namespace MissCore.Bot
{
    public record Search : BotEntityAction
    {
        public string Placeholder { get; set; }

    }
    public record Filter(int skip, int take, string predicat);
    public record Search<TEntity> : Search
    {
        public Filter Filter { get; init; }

        public SQL ToQuery(Filter filter)
            => string.Format(Payload, filter.skip, filter.take, filter.predicat);
    }
}
