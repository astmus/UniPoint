using System.Text.Json;

namespace MissBot.Abstractions.Utils
{
    public struct Position
    {
        public uint Current { get; set; }
        public uint Forward
            => ++Current;
        public uint Bask
            => Current > 0 ?  --Current : Current;        
    }   
}
