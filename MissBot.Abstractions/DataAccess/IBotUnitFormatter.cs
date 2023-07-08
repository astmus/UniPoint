using System.Linq.Expressions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Bot;

namespace MissBot.Abstractions.DataAccess
{
    public interface IBotUnitFormatter
    {        
        IUnitRequest<TBotUnit> FormatParameters<TBotUnit>(TBotUnit sp) where TBotUnit:BaseBotUnit, IBotUnit;
    }
}
