using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace RefitApi
{
    [JsonConverter(typeof(ProgramMessage))]
    public enum ProgramMessage
    {
        fg,df,y6tryu,f6789,g69,gggg8
    }

    [JsonConverter(typeof(ProgramMessageCommand))]
    public record ProgramMessageCommand(int i, int f);
    
}
