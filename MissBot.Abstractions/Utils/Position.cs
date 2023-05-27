using System.Text.Json;

namespace MissBot.Abstractions.Utils
{
    public struct Position
    {
        public uint Current { get; set; }
        public uint Forward
            => ++Current;
        public uint Count { get; set; }
        public uint Back
            => Current > 0 ?  --Current : Current;        
    }   
}
