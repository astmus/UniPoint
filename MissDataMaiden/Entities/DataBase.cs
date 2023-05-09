using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Handlers;
using MissCore.Entities;
using MissDataMaiden.Commands;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace MissDataMaiden.Entities
{
    [JsonConverter(typeof(DBActionConverter))]
    public enum DBAction : byte
    {
        Unknown = 0,
        Details,
        Info,
        Restore,
        Delete
    }

    public record InlineDataBase : InlineUnit<DataBase>
    {
        public new string Id { get => Get<string>(); set => Set(value); }
        public string Name { get => Get<string>(); set => Set(value); }
        public float? Size { get; set; }
        public string Created { get=> Get<string>(); set => Set(value); }
    }
    public record DataBase : Unit<ChosenInlineResult>
    {
        public string Id { get => Get<string>(); set => Set(value); }
        public string Name { get => Get<string>(); set => Set(value); }
        public float? Size { get; set; }
        public string Created { get => Get<string>(); set => Set(value); }
        public List<EntityAction<DataBase>> Commands { get => Get<List<EntityAction<DataBase>>>(); set => Set(value); }
    }

    public record DataBaseInfo : DataBase
    {
        public string Info { get => Get<string>(); set => Set(value); }
        public string DBName { get => Get<string>(); set => Set(value); }
        public string Status { get => Get<string>(); set => Set(value); }
        public string State { get => Get<string>(); set => Set(value); }
        public int DataFiles { get => Get<int>(); set => Set(value); }
        public int DataMB { get => Get<int>(); set => Set(value); }
        public int LogFiles { get => Get<int>(); set => Set(value); }
        public int LogMB { get => Get<int>(); set => Set(value); }
        public string RecoveryModel { get => Get<string>(); set => Set(value); }
        public string LastBackup { get => Get<string>(); set => Set(value); }
        public string IsReadOnly { get => Get<string>(); set => Set(value); }
     //   public List<EntityAction<DataBase>> Commands { get => Get<List<EntityAction<DataBase>>>(); set => Set(value); }
    }
}
