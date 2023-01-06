using MissBot.Application.Common.Interfaces;

namespace MissBot.Infrastructure.Services;
public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
