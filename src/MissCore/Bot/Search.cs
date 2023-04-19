using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissCore.Entities;

namespace MissCore.Bot
{
    public record Search : ValueUnit
    {
        public string Action { get; set; }
        public string Placeholder { get; set; }

    }
    public record Search<TEntity> : ISQL
    {
        public string Cmd { get; init; }
        public object[]? Params { get; init; }
        public string Entity
            => this.ToString();

        public string Command { get; init; }
    }
}
