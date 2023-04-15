using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissBot.Abstractions;
using MissDataMaiden.Commands;

namespace MissDataMaiden.Entities
{
    public record DataBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Size { get; set; }
        public string Created { get; set; }   
    }
}
