using System.Text.Json;

namespace MissBot.Abstractions.Utils
{
    public record struct Position
    {
        int value;

        public int Current => value;
        public int Forward()
            =>  ++value;              
        public int Back()
            => value > 0 ?  --value : value;        
    }   
}
