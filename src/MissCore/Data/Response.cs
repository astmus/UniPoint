using BotService.Internal;
using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Entities;
using MissBot.Entities;
using MissCore.Collections;

namespace MissCore.Data
{

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record Response<T>(IHandleContext Context = default) : BaseResponse<T>(Context) where T : BaseUnit
    {
        Message Message
            => Context.Take<Message>();
        public Message<T> CurrentMessage { get; protected set; }
        public override int Length
            => Content.ToString().Length;
        protected override JsonConverter CustomConverter
            => Context.GetBotService<UnitConverter<T>>();
        public override async Task Commit(CancellationToken cancel)
        {
            //if (string.IsNullOrEmpty(Content)) return;
            if (Content == null) return;
            CurrentMessage = await Context.BotServices.Client.SendQueryRequestAsync(this, cancel).ConfigureAwait(false);
            //Content = string.Empty;
            Content = default;
        }
        [JsonConverter(typeof(UnitConverter<BaseUnit>))]
        public override IUnit<T> Content { get => base.Content; set => base.Content = value; }


        // public override T Content { get => base.Content; set => base.Content = value; }

        public override IActionsSet Actions
            => Content.Actions;

        //public override void Write<TData>(TData unit) where TData:T



        //{
        //  
        //}
        

        public void WriteMetadata<TMetaData>(TMetaData meta) where TMetaData : class, IMetaData
        {
            //Content += string.Join(" ", meta.Keys)+'\n';
        }

        public override IResponse<T> InputData(string description, IActionsSet options = null)
        {
           // Content = description;
            //Actions = options;
            return this;
        }

        public override IResponse CompleteInput(string message)
        {
            //Content = message;

          /*  if (Actions is IChatActionsSet set)
                Actions = set.RemoveKeyboard();*/
                
            return this;
        }


    }
}


