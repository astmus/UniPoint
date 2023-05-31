using System.Linq.Expressions;
using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions.DataAccess
{
    public interface IBotUnitFormatter
    {        
        IUnitRequest<TBotUnit> FormatParameters<TBotUnit>(TBotUnit sp) where TBotUnit:BaseUnit, IBotUnit;
    }
}
