using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissBot.Abstractions.DataContext;
using MissBot.Abstractions.Entities;
using MissCore.Collections;

namespace MissBot.Utils
{

    public class BotUnitFormatProvider : IBotUnitFormatProvider
    {
        private readonly IHandleContext _context;        

        public ICustomFormatter GetCriteriaFormatter(IBotServicesProvider sp)
            => ActivatorUtilities.GetServiceOrCreateInstance<CriteriaFormatter>(_context.BotServices);
        public ICustomFormatter GetUnitFormatter(IBotServicesProvider sp)
            => ActivatorUtilities.GetServiceOrCreateInstance<UnitFormatter>(_context.BotServices);

        public object? GetFormat(Type? formatType) => formatType switch
        {
            var f when f == typeof(Unit) => GetUnitFormatter(_context.BotServices),            
            _ => null
        };
    }
}

