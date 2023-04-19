using MissBot.Attributes;
using MissBot.Abstractions;
using MissDataMaiden.Queries;
using Duende.IdentityServer.Services;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MissCore.Entities;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using MediatR;
using MissBot.Common;
using MissBot.Extensions.Response;
using MissBot.Abstractions.DataAccess;
using Telegram.Bot.Types;
using BotService;
using MissCore.Bot;
using MissBot.DataAccess.Sql;

namespace MissDataMaiden.Commands
{
    [JsonObject]
    public class Disk : BotCommand<Disk>, IBotCommand
    {
        static  Disk()
        {
            
        }
        public override string CommandName => nameof(Disk);

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
        static DiskCommandHandler()
        {
            Unit<Disk>.Meta.Data["2"] =
            //Disk.Dto.MetaData = Content => 
            $"{nameof(Disk.Dto.Name).Shrink(10)}    {nameof(Disk.Dto.Drive)}    {nameof(Disk.Dto.Free)}    {nameof(Disk.Dto.Used)}    {nameof(Disk.Dto.Total)}    {nameof(Disk.Dto.Perc)}";
            Unit<Disk>.Meta.Data["1"] = nameof(Disk);
        }


        //public override async  Task BeforeComamandHandle(IContext<Disk> context)
        //{
        //    Disk.CommandResult response = context.Scope.Result;

        //    await response.SendHandlingStart(); 
        //}

        public override async Task HandleCommandAsync(Disk command, IContext<Disk> context)
        {
            IResponse<Disk> response = context.CreateResponse(command);
            //Unit<Disk>.MetaData
            response.WriteMetadata(Unit<Disk>.Meta);
            IList<Disk.Dto> results = await repository.HandleQueryItemsAsync<Disk.Dto>(command.Payload);
            //query = SqlQuery<Disk.Dto>.Instance with { sql = command.Payload, connectionString = config.GetConnectionString("Default") };
            //var results = await query.Handle().ConfigFalse();
            foreach (var obj in results)
            {
                //obj.AddMetaData();
                response.Write(obj);
            }
            await response.Commit(default);
        }  
    }
}
