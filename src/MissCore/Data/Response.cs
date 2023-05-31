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
            if (string.IsNullOrEmpty(Content)) return;
            
            CurrentMessage = await Context.BotServices.Client.SendQueryRequestAsync(this, cancel).ConfigureAwait(false);
            Content = string.Empty;
        }

        public override void Write<TUnitData>(TUnitData unit)
        {
            WriteUnit(unit, IUnit.Formats.Line);
        }
      
        public override void Write<TUnitData>(IEnumerable<TUnitData> units) 
        {
            foreach (var unit in units)
                WriteUnit(unit, IUnit.Formats.Table | IUnit.Formats.PropertyNames);
        }

        protected virtual Response<T> WriteUnit(IUnit unit, IUnit.Formats format)
        {
            Content += unit?.Format(format);
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


