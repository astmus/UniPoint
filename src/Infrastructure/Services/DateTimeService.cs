using MissBot.Common.Interfaces;

namespace MissBot.Infrastructure.Services;
public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
