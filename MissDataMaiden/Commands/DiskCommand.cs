using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.DataAccess.Sql;
using MissBot.Extensions.Response;
using MissCore.Collections;
using MissCore.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MissDataMaiden.Commands
{
    [JsonObject]
    public record Disk : BotCommandUnit
    {
        public override string CommandAction => nameof(Disk);

        public override string Entity
            => nameof(BotCommand);

        [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
        public record Dto : Unit<Disk>
        {
            public string Name { get; set; }
            public string Drive { get; set; }
            public double Free { get; set; }
            public double Used { get; set; }
            public double Total { get; set; }
            public string Perc { get; set; }
        }

    }

    

    public class DiskCommandHandler : BotCommandHandler<Disk>
    {
        private readonly IConfiguration config;
        private readonly IJsonRepository repository;        

        public DiskCommandHandler(IJsonRepository repository)
        {
            this.repository = repository;
        }

        
        static DiskCommandHandler()
        {
            //Unit<Disk>.Instance.MetaData["2"] =
            //Disk.Dto.MetaData = Content => 
            //$"{nameof(Disk.Dto.Name).Shrink(10)}    {nameof(Disk.Dto.Drive)}    {nameof(Disk.Dto.Free)}    {nameof(Disk.Dto.Used)}    {nameof(Disk.Dto.Total)}    {nameof(Disk.Dto.Perc)}";
            //Unit<Disk>.Instance.MetaData["1"] = nameof(Disk);

        }
        
        public async override Task HandleCommandAsync(Disk command, IHandleContext context, CancellationToken cancel = default)
        {

            var response = new Response<Disk>(context);

            var res = await repository.ReadUnitDataAsync(command);

            var r = new MetaCollection(res);
            foreach (var item in res)
            {
                var da = MetaData.Parse(item);
                response.Text += da.Value;
       
                // var unit = item.ToObject<Unit<Disk>>();
                //response.Write(unit);
            }
            //if (await sqlQuery.GetResponseItemsAsync<Disk.Dto>(repository) is ICollection<Disk.Dto> result)
            //{
            //    foreach (var obj in result)
            //    {
            //        //obj.AddMetaData();
            //        response.Write(obj);
            //    }
            //}
            //else if (sqlQuery.Result is RequestResult error)
            //    response.WriteError(error);

            await response.Commit(default);
        }


    }
}
