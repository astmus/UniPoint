using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissCore.Abstractions;

namespace MissCore
{
    internal class BotServicesProvider : IBotServicesProvider
    {
        public IBotServicesProvider CreateScope()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public object? GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }
}
