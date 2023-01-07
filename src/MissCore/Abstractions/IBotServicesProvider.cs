using System;

namespace MissCore.Abstractions
{
    public interface IBotServicesProvider : IServiceProvider, IDisposable
    {
        IBotServicesProvider CreateScope();
    }
}
