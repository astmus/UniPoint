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

    public record InlineDataBase : InlineUnit<InlineDataBase>
    {
        public string Name { get=> Get<string>(); set=>Title = Set(value); }
        public float Size { get; set; }
        public string Created { get=> Get<string>(); set=> Description = Set(value); }
    }
        public record DataBase
    {        
        public int Id { get; set; }
        public string Name { get; set; }
        public float Size { get; set; }
        public string Created { get; set; }
    }
}
