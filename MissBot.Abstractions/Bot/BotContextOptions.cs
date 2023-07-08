namespace MissBot.Abstractions.Bot
{
    //public record BotContextOptions(string? connectionString, string? driverName = ProviderName.SqlServer2022);
    public class BotContextOptions
    {
        public const string ContextOptions = nameof(BotContextOptions);
        public string ConnectionString { get; set; } = string.Empty;
        public string DataProvider { get; set; } = string.Empty;
    }
}
