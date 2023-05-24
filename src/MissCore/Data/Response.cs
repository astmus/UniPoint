using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissBot.Entities;
using MissCore.Collections;

namespace MissCore.Data
{

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record Response<T>(IHandleContext Context = default) : BaseResponse<T>(Context), IResponse<T>
    {
        Message Message
            => Context.Take<Message>();
        public Message<T> CurrentMessage { get; protected set; }
        public int Length
            => Content.Length;

        public async Task Commit(CancellationToken cancel)
        {
            if (Content == String.Empty) return;
            
            CurrentMessage = await Context.BotServices.Client.SendQueryRequestAsync(this, cancel);
            Content = String.Empty;
        }

        public void Write<TUnitData>(TUnitData unit) where TUnitData : Unit, IUnit<T>
        {
            WriteUnit(unit);
        }

        public void WriteResult<TUnitData>(TUnitData units) where TUnitData :  IEnumerable<IUnit<T>>
        {
            //foreach (var unit in units)
            //    Write(unit);
        }
        public void Write<TUnitData>(IEnumerable<TUnitData> units) where TUnitData : Unit, IUnit<T>
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
    }
}


