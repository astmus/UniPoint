using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissCore.Abstractions;

namespace MissCore
{
    public class BotServicesProvider : IBotServicesProvider
    {
        IServiceProvider sp;
        public BotServicesProvider(IServiceProvider spr)
            => sp = spr;       

        public T GetRequiredService<T>()
            => sp.GetRequiredService<T>();

        public T GetService<T>()
            => sp.GetService<T>();

        public object? GetService(Type serviceType)
            => sp.GetService(serviceType);
    }
}
