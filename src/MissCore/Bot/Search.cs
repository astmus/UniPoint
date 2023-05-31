namespace MissCore.Bot
{
    public record Search : UnitRequest
    {
        public string Query { get; set; }
        public Paging Pager { get; set; }
    }
}
