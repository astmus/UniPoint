using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissBot.Abstractions.DataAccess;

namespace MissBot.Abstractions.Entities
{
    public abstract record SQLEntity : BotEntity,  ISQLUnit
    {
        public abstract SQLCommand Command { get; }
        public SQLResult Result { get; set; }
    }
}
