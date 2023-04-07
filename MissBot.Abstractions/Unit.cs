using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissBot.Abstractions
{
    
    public abstract record BaseEntity
    {
        public record Unit<TUnit> : BaseEntity
        {
            public TUnit Value { get; protected set; }
        };

        //public static implicit operator BaseBotCommand(string cmd)
        //    => new BaseBotCommand($"/{cmd}".Replace("//", "/").ToLower());
        //public static implicit operator string(BaseBotCommand cmd)
        //    => $"/{cmd.Command}".Replace("//", "/").ToLower();

        //public static bool operator ==(string commandA, BaseBotCommand commandB)
        //=> commandA?.EndsWith(commandB?.Command?.ToLower()) == true ||
        //            commandB?.Command.ToLower().EndsWith(commandA ?? string.Empty) == true;

        //public static bool operator !=(string commandA, BaseBotCommand commandB)
        //    => commandA?.EndsWith(commandB?.Command?.ToLower()) == false &&
        //         commandB?.Command?.ToLower().EndsWith(commandA ?? string.Empty) == false;

    }
}
