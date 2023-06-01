using MissBot.Abstractions;
using MissCore.Data;
using MissDataMaiden.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MissDataMaiden.Actions
{
    public record DBRestore : BotUnitAction<DataBase>;
    public record DBDelete : BotUnitAction<DataBase>;
    public record DBInfo : BotUnitAction<DataBase>;
    

}
