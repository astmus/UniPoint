using MissBot.Abstractions;
using MissBot.Entities;
using MissCore.Collections;

namespace MissCore.Data
{

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record Response<T>(IHandleContext Context = default) : BaseResponse<T>(Context), IResponse<T>
    {
        Message message
            => Context.Take<Message>();



        public async Task Commit(CancellationToken cancel)
        {               
            await Context.BotServices.Client.SendQueryRequestAsync(this, cancel);
        }


        public void Write<TUnitData>(TUnitData unit) where TUnitData : class, IUnit<T>
        {
            WriteUnit(unit);
        }

        public void WriteResult<TUnitData>(TUnitData units) where TUnitData :  IEnumerable<IUnit>
        {
            //foreach (var unit in units)
            //    Write(unit);
        }
        public void Write<TUnitData>(IEnumerable<TUnitData> units) where TUnitData : class, IUnit<T>
        {
            //foreach (var unit in units)
            //    Write(unit);
        }

        protected virtual Response<T> WriteUnit(IUnit unit)
        {
            Text += unit?.ToString();
            return this;
        }

        public void WriteMetadata<TMetaData>(TMetaData meta) where TMetaData : class, IMetaData
        {
            Text += Unit<T>.ParseTyped(meta) + "\n";
        }

        public void WriteError<TUnitData>(TUnitData unit) where TUnitData : class, IUnit
        {
            Text += unit.ToString();
        }


    }
}


