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
using Telegram.Bot.Types;

namespace MissCore.Features
{
    public record Search : BotRequest
    {

    }
    public record Search<TEntity> : ValueUnit
    {
        public Filter Filter { get; set; }
    }
    public readonly record struct Filter(int skip, int take, string predicat);

}
