using MissCore.Abstractions;
using Newtonsoft.Json;
using TG = Telegram.Bot.Types.Update;
/// <summary>
/// This object represents a message.
/// </summary>
public class Update : TG, IUpdateInfo
{
    public long ChatId { get; }
    public long UserId { get; }

    [JsonProperty("update_id", Required = Required.Always)]
    public uint UpdateId { get; init; }
        
    public bool IsHandled { get; set; }

    public string GetId()    
        => $"{nameof(ChatId)}:{ChatId} {nameof(UserId)}:{UserId}";
    
}

public class Update<TEntity> : Update, IUpdateInfo
{
    
}
