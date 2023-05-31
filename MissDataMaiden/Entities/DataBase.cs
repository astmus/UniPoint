using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissBot.Entities.Results;
using MissCore;
using MissCore.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MissDataMaiden.Entities
{
    [JsonConverter(typeof(DbActionConverter))]
    public enum DbAction : byte
    {
        Unknown = 0,
        Details,
        Info,
        Restore,
        Delete
    }
    public record Info : DataBase
    {
        public string Id { get; set; }
        public string Unit { get; set; }
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

    
    public record DataBase : Unit<ChosenInlineResult>, IBotEntity
    {
        public override object Identifier => Id;
        public string Id { get; set ; }
        public string Name { get; set ; }
        public float? Size { get; set; }
        public string Created { get; set ; }

        public override string Entity
            => nameof(DataBase);
        [JsonIgnore]
        public string Unit { get; set; }
    }

    public record DataBaseInfo : BotUnit
    {
        public string Id { get; set; }        
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
        public override string Format(IUnit.Formats? format = null)
        {
            return this.ToString();
        }
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
