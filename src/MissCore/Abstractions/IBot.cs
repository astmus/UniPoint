namespace MissCore.Abstractions
{
    public interface IBot
    {
        IBotServicesProvider BotServices { get; }

        //IBotOptions Options { get; set; }
        //IBotConnection Connection { get; }
        //  ICollection<IBotCommandInfo> Commands { get; }
    }
}
