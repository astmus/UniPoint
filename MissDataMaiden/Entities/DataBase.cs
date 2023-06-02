using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissBot.Entities.Results;
using MissCore;
using MissCore.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MissDataMaiden.Entities
{
    public record Info : Unit<DataBase>
    {
        public override string UnitKey
            => base.UnitKey + nameof(Info);

        public string DBName { get; set; }
        public string Status { get; set; }
        public string State { get; set; }
        public int DataFiles { get; set; }
        public int DataMB { get; set; }
        public int LogFiles { get; set; }
        public int LogMB { get; set; }
        public string RecoveryModel { get; set; }
        public string LastBackup { get; set; }
        public string IsReadOnly { get; set; }
    }

    
    public record DataBase : Unit, IBotEntity
    {
        [JsonProperty("Unit", Order = int.MinValue)]
        public override string UnitKey
            => nameof(DataBase);

        public override object Identifier
            => Id;
           
        [JsonProperty]
        public string Id { get; set; }
        public string Name { get; set ; }
        public float? Size { get; set; }
        public string Created { get; set ; }        
    }
    [JsonObject]
    public record Progress([JsonProperty(Order = int.MinValue)] byte Completed, [JsonProperty(Order = int.MinValue+1)]  TimeSpan Elapsed) { }
    public record DataBaseInfo : BotUnit
    {
        public override object Identifier
            => Id;

        public string Id { get; set; }        
        public string Status { get; set; }
        public string State { get; set; }
        public int DataFiles { get; set; }
        public int DataMB { get; set; }
        public int LogFiles { get; set; }
        public int LogMB { get; set; }
        public string RecoveryModel { get; set; }
        public string LastBackup { get; set; }
        public string IsReadOnly { get; set; }
        //public string Id { get => Get<string>(); set => Set(value); }
        //public string Info { get => Get<string>(); set => Set(value); }
        //public string DBName { get => Get<string>(); set => Set(value); }
        //public string Status { get => Get<string>(); set => Set(value); }
        //public string State { get => Get<string>(); set => Set(value); }
        //public int DataFiles { get => Get<int>(); set => Set(value); }
        //public int DataMB { get => Get<int>(); set => Set(value); }
        //public int LogFiles { get => Get<int>(); set => Set(value); }
        //public int LogMB { get => Get<int>(); set => Set(value); }
        //public string RecoveryModel { get => Get<string>(); set => Set(value); }
        //public string LastBackup { get => Get<string>(); set => Set(value); }
        //public string IsReadOnly { get => Get<string>(); set => Set(value); }
        //public List<EntityAction<DataBase>> Commands { get => Get<List<EntityAction<DataBase>>>(); set => Set(value); }
    }
}
