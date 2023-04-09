using MissBot.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MissCore.Entities
{

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record BaseBotCommand : BaseEntity, IBotCommandData, IBotCommandInfo
    {
        /// <summary>
        /// Text of the command, 1-32 characters. Can contain only lowercase English letters, digits and underscores.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Command { get; set; } = default!;
        /// <summary>
        /// Description of the command, 3-256 characters.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Description { get; set; } = default!;

        static Type _CmdType;
        public Type CmdType
            => _CmdType ??= GetType();

        public string Payload { get; set; }
        public string[] Params { get; set; }
        
    }

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record BotCommand<TResult> : BaseBotCommand, IBotCommand where TResult : BotCommand<TResult>.Unit
    {
        public CommandResult Result { get; protected set; } = new CommandResult();        

        public record CommandResult : BotCommand<Unit>.Union
        {
            public string ErrorMessage { get; set; }
            //Chat CurrentChat { get; set; }

            //Message<Result> response;
            //public  async Task SendHandlingStart()
            //{
            //    response = await Context.BotServices.GetRequiredService<IBotClient>().SendQueryRequestAsync(new ResponseRequest<Result>(Context.Get<Telegram.Bot.Types.Chat>().Id));
            //    CurrentChat = response.Chat;
            //    await Context.BotServices.GetRequiredService<IBotClient>().SendCommandAsync(CurrentChat.SetAction(Telegram.Bot.Types.Enums.ChatAction.Typing));
            //}
            

            //public async Task WriteAsync<T>(T data, CancellationToken cancel) where T : class
            //{
            //    await SendHandlingStart();                
            //    response.Result = this;
            //    var responseMessage = await Context.BotServices.GetRequiredService<IBotClient>().SendQueryRequestAsync(response.Update(),cancel);
            //    Context.Set(responseMessage);
            //}
          
            //public async Task UpdateAsync<T>(T data, CancellationToken cancel) where T : class
            //{
            //    if (ctx.Get<Message<T>>() is Message<T> responseMessage)
            //    {
            //        //responseMessage.Result = data;
            //        var updatedMessage = await ctx.BotServices.GetRequiredService<IBotClient>().SendQueryRequestAsync(new EditRequest<T>(responseMessage.Chat.Id, responseMessage.MessageId, data.ToString()));
            //        ctx.Set(updatedMessage);
            //    }
            //}

            //public async Task UpdateAsync(CancellationToken cancel)
            //{
            //    if (Context.Get<Message<Result>>() is Message<Result> responseMessage)
            //    {
            //        var chatResponse = new ResponseRequest<Result>(CurrentChat.Id);
            //        chatResponse.Message.Result = this;
            //        /*var updatedMessage = */await Context.BotServices.GetRequiredService<IBotClient>().SendCommandAsync(chatResponse, cancel);
            //        //ctx.Set(updatedMessage);
            //    }
            //}


            //public Task SendAsync(TData data, CancellationToken cancel)
            //{
            //    return WriteAsync(this, cancel);
            //}

            //public override Task Commit(CancellationToken cancel)
            //{               
            //    return WriteAsync(this, cancel);
            //}

          

            //public async Task<TResult> CreateResponseAsync<TResult>(IHandleContext context) where TResult:IResponseResultUnit
            //{
            //    var response = await ctx.BotServices.GetRequiredService<IBotClient>().SendQueryRequestAsync(new ResponseRequest<TResult>(ctx.Get<Telegram.Bot.Types.Chat>().Id));
            //    return response.Result;

            //}


        }
    }
}
