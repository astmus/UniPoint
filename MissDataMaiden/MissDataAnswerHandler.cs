using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Entities.Results;
using MissBot.Handlers;
using MissCore;
using MissDataMaiden.Entities;


namespace MissDataMaiden
{
    internal class MissDataAnswerHandler : InlineAnswerHandler
    {
        IJsonRepository repository;
        public MissDataAnswerHandler(IJsonRepository jsonRepository)
        {
            repository = jsonRepository;
        }
        //[JsonObject(MemberSerialization = MemberSerialization.OptOut)]
        //record DataBaseRequest : SQL<DataBase>.Query<DataBaseInfo>
        //{
        //    public DataBaseRequest()
        //    {

        //    }
        //}

        public override async Task HandleResultAsync(ChosenInlineResult result, IHandleContext context)
        {
            int id = result.Query.Length > 0 ? int.Parse(result.ResultId.Replace(result.Query, "")) : int.Parse(result.ResultId);
            string strid = result.Query.Length > 0 ? result.ResultId.Replace(result.Query, "") : result.ResultId;

            var cmd = BotUnit<DataBase>.Query(f => f.Id = strid);

     

            //sql.ContentPropertyName = nameof(DataBase.Detail);
            var details = await repository.HandleCommandAsync<DataBase>(cmd);
            //var response = context.CreateResponse(result);
            ////response.WriteMetadata(unit);
            //response.Write(details );
            //await response.Commit(default);

            int i = 0;
        }
    }
}

