using MissBot.Abstractions;
using MissCore;
using Newtonsoft.Json;
using TG = Telegram.Bot.Types.Update;

public class Update : TG, IUpdateInfo
{
    [JsonProperty("update_id", Required = Required.Always)]
    public uint UpdateId { get; init; }
    public bool? IsHandled { get; set; }    
}

