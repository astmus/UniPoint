using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Common;
using MissBot.DataAccess.Sql;
using MissBot.Extensions.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MissDataMaiden.Commands
{
    [JsonObject]
    public record Disk : BotCommandUnit
    {
        static  Disk()
        {
            //Unit<Disk>.Meta.Instance["Title"] = $"{nameof(Disk.Dto.Name).Shrink(10)}    {nameof(Disk.Dto.Drive)}    {nameof(Disk.Dto.Free)}    {nameof(Disk.Dto.Used)}    {nameof(Disk.Dto.Total)}    {nameof(Disk.Dto.Perc)}";
        }
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

    public record DiskResponse : Response<Disk>
    {
        protected override Response<Disk> WriteUnit(ValueUnit unit) => unit switch
        {
            Disk.Dto du => WriteDataUnit(du),
            _ => base.WriteUnit(unit)
        };

        Response<Disk> WriteDataUnit(Disk.Dto data)
        {            
            Text += $"{data.Name.Shrink(10)}{data.Drive}  {data.Free}  {data.Used}   {data.Total}    {data.Perc}".AsCodeTag().LineTag();
            return this;
        }
    }

    public class DiskCommandHandler : BotCommandHandler<Disk>
    {
        private readonly IConfiguration config;
        private readonly IJsonRepository repository;
        SQL<Disk.Dto> query;

        public DiskCommandHandler(IJsonRepository repository)
        {
            //SQL < Disk.Dto >.
                this.repository = repository;
        }
        public readonly record struct Id(object id);
        static DiskCommandHandler()
        {
            
            //Unit<Disk>.Instance.MetaData["2"] =
            //Disk.Dto.MetaData = Content => 
            //$"{nameof(Disk.Dto.Name).Shrink(10)}    {nameof(Disk.Dto.Drive)}    {nameof(Disk.Dto.Free)}    {nameof(Disk.Dto.Used)}    {nameof(Disk.Dto.Total)}    {nameof(Disk.Dto.Perc)}";
            //Unit<Disk>.Instance.MetaData["1"] = nameof(Disk);

        }

        

        public override async Task HandleCommandAsync(Disk command, IContext<Disk> context)
        {
            IResponse<Disk> response = context.CreateResponse(command);
            //Unit<Disk>.MetaData
            //response.WriteMetadata(Unit<Disk.Dto>.TypeMeta);
            var sqlQuery = SQLUnit<Disk.Dto>.Instance with { Command = command.Payload };
            if (await sqlQuery.GetResponseItemsAsync<Disk.Dto>(repository) is ICollection<Disk.Dto> result)
            {
                foreach (var obj in result)
                {
                    //obj.AddMetaData();
                    response.Write(obj);
                }
            }
            else if (sqlQuery.Result is RequestResult error)
                response.WriteError(error);
            
            await response.Commit(default);
        }  
    }
}
