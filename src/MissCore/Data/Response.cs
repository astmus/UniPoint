using MissBot.Abstractions;
using MissBot.Entities.Common;

namespace MissBot.Entities.Response
{

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record Response<T>(IHandleContext Context = default) : BaseResponse<T>(Context), IResponse<T>
    {
        Message message
            => Context.Take<Message>();



        public async Task Commit(CancellationToken cancel)
        {
            Caption = "!!!!!";
            await Context.BotServices.Client.SendQueryRequestAsync(this, cancel);
        }


        public void Write<TUnitData>(TUnitData unit) where TUnitData : Unit<T>
        {
            WriteUnit(unit);
        }
                                                                  
        public void WriteResult<TUnitData>(TUnitData units) where TUnitData : IEnumerable<Unit>
        {
            //foreach (var unit in units)
            //    Write(unit);
        }
        public void Write<TUnitData>(IEnumerable<TUnitData> units) where TUnitData : Unit<T>
        {
            //foreach (var unit in units)
            //    Write(unit);
        }

        protected virtual Response<T> WriteUnit(Unit unit)
        {
            Text += unit?.ToString();
            return this;
        }

        public void WriteMetadata<TMetaData>(TMetaData meta) where TMetaData : MetaData
        {
            Text += Unit<T>.ParseTyped(meta) + "\n";
        }

        public void WriteError<TUnitData>(TUnitData unit) where TUnitData : Unit
        {
            Text += unit.ToString();
        }


    }      
}


