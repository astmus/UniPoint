using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissCore.Collections;

namespace MissBot.Utils
{

    public class BotUnitFormatProvider : IBotUnitFormatProvider
    {
        private readonly IHandleContext _context;

        public BotUnitFormatProvider(IHandleContext context)
            => _context = context;

        public ICustomFormatter GetCriteriaFormatter()
            => ActivatorUtilities.GetServiceOrCreateInstance<CriteriaFormatter>(_context.BotServices);
        public ICustomFormatter GetUnitFormatter()
            => ActivatorUtilities.GetServiceOrCreateInstance<UnitFormatter>(_context.BotServices);

        public object? GetFormat(Type? formatType) => formatType switch
        {
            var f when f == typeof(ICriteria) => GetCriteriaFormatter(),
            var f when f == typeof(Unit) => GetUnitFormatter(),            
            _ => null
        };
    }
}

