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
        public async Task Commit(CancellationToken cancel)
            => CurrentMessage =  await Context.BotServices.Client.SendQueryRequestAsync(this, cancel);

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
            Text += unit?.Format();
            return this;
        }

        public void WriteMetadata<TMetaData>(TMetaData meta) where TMetaData : class, IMetaData
        {
            Text += string.Join(" ", meta.Keys)+'\n';
        }

        public void WriteError<TUnitData>(TUnitData unit) where TUnitData : class, IUnit
        {
            Text += unit.ToString();
        }
    }
}


