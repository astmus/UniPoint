using System;

namespace MissCore.Abstractions
{
    public interface IBotCommandData
    {
        string Payload { get; set; }
        string[] Params { get; set; }        
    }

    public interface IBotCommandInfo
    {
        Type CmdType { get; }
        string Command { get; }
        string Description { get; }
    }
}
