using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissCore.Data.Context;

namespace MissCore.DataAccess
{
    public class ResponseDataContext : Context
    {

        
        protected override string GetId<T>()
        {
            return base.GetId<T>();
        }

    }
}
