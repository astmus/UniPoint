using MissCore.DataAccess;

namespace MissCore.Bot
{
    public record Search : UnitRequest
    {
        public string Query { get; init; }
        public Paging Pager { get; init; }
    }
}
