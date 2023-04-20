using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissBot.Abstractions;
using MissBot.Handlers;
using MissDataMaiden.Commands;

namespace MissDataMaiden.Entities
{
        public enum DBAction : byte
        {
            Details,
            Info,
            Restore,
            Delete
        }

    public record InlineDataBase : DataBase
    {        
        public string Title => Name;
        public string Description => Created;
    }
        public record DataBase : ValueUnit
    {        
        public string Id { get; set; }
        public string Name { get; set; }
        public float? Size { get; set; }
        public string Created { get; set; }
    }
}
