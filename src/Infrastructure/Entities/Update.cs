using MissCore.Abstractions;
using TG = Telegram.Bot.Types.Update;
/// <summary>
/// This object represents a message.
/// </summary>
public class Update : TG, IUpdateInfo
{
    public long ChatId { get; }
    public long UserId { get; }
    public uint UpdateId { get; }
    public bool IsHandled { get; set; }

    public string GetId()    
        => $"{nameof(ChatId)}:{ChatId} {nameof(UserId)}:{UserId}";
    
}
