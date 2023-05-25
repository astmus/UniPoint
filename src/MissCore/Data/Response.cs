using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Entities;
using MissBot.Entities;
using MissCore.Collections;

namespace MissCore.Data
{

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record Response<T>(IHandleContext Context = default) : BaseResponse<T>(Context)
    {
        Message Message
            => Context.Take<Message>();
        public Message<T> CurrentMessage { get; protected set; }
        public override int Length
            => Content.Length;

        public override async Task Commit(CancellationToken cancel)
        {
            if (Content == String.Empty) return;
            
            CurrentMessage = await Context.BotServices.Client.SendQueryRequestAsync(this, cancel);
            Content = String.Empty;
        }

        public override void Write<TUnitData>(TUnitData unit)
        {
            WriteUnit(unit);
        }

        public override void WriteResult<TUnitData>(TUnitData units) 
        {
            //foreach (var unit in units)
            //    Write(unit);
        }
        public override void Write<TUnitData>(IEnumerable<TUnitData> units) 
        {
            //foreach (var unit in units)
            //    Write(unit);
        }

        protected virtual Response<T> WriteUnit(IUnit unit)
        {
            Content += unit?.Format();
            return this;
        }

        public void WriteMetadata<TMetaData>(TMetaData meta) where TMetaData : class, IMetaData
        {
            Content += string.Join(" ", meta.Keys)+'\n';
        }

        public override IResponse<T> InputData(string description, IActionsSet options = null)
        {
            Content = description;
            Actions = options;
            return this;
        }

        public override IResponse CompleteInput(string message)
        {
            Content = message;

            if (Actions is IChatActionsSet set)
                Actions = set.RemoveKeyboard();
                
            return this;
        }
    }
}


