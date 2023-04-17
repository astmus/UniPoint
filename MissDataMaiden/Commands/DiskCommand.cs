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
using static MissCore.Bot.BotCore;

namespace MissDataMaiden.Commands
{
    [JsonObject]
    public class Disk : BotCommand<Disk>, IBotCommand
    {
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
        public record Request(string connection = null)
            : SqlQuery<Disk>.Request($"select * from ##BotCommands c INNER JOIN ##BotActionPayloads a ON c.Command = a.EntityAction where Command = /{nameof(Disk).ToLower()}", connection);

        public record Handler : SqlQuery<Disk>        {
         
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
        private readonly IRepository<BotCommand> commandsRepository;
        SqlQuery<Disk.Dto> query;
        static Disk disk;
        public DiskCommandHandler(IConfiguration config, IRepository<BotCommand> commandsRepository)
        {            
            this.config = config;
            this.commandsRepository = commandsRepository;
        }

        public override Disk Command
        {
            get
            {
                if (disk is Disk cmd)
                    return cmd;
                    
                SqlQuery<Disk>.Sample.Command ??= nameof(Disk);
                var diskRequest = Cmd<BotCommand>.BotCommand<Disk>(nameof(disk));
                return disk = commandsRepository.GetAll<Disk>().FirstOrDefault(SqlQuery<Disk>.Sample);
            }
        }
        
        //public override async  Task BeforeComamandHandle(IContext<Disk> context)
        //{
        //    Disk.CommandResult response = context.Scope.Result;

        //    await response.SendHandlingStart(); 
        //}

        public override async Task RunAsync(Disk command, IContext<Disk> context)
        {
            IResponse<Disk> response = context.CreateResponse(command);


            var metaUnit = Disk.Dto.CreateMetaUnit($"{nameof(Disk.Dto.Name).Shrink(10)}    {nameof(Disk.Dto.Drive)}    {nameof(Disk.Dto.Free)}    {nameof(Disk.Dto.Used)}    {nameof(Disk.Dto.Total)}    {nameof(Disk.Dto.Perc)}");
            
            response.Write(metaUnit);

            query = SqlQuery<Disk.Dto>.Instance with { sql = command.Payload, connectionString = config.GetConnectionString("Default") };
            var results = await query.Handle().ConfigFalse();
            foreach (var obj in results)
            {
                response.Write(obj);
            }
            await response.Commit(default);
        }  
    }
}
