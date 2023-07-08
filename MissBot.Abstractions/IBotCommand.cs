
using MissBot.Abstractions.Actions;
using MissBot.Entities.Abstractions;
using MissBot.Entities.Results.Inline;

namespace MissBot.Abstractions
{
	public interface IBotCommand : IBotEntity
    {
        string Command { get; }
        string Description { get; }
    }
}
