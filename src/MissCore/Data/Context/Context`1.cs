using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissCore.Abstractions;

namespace MissCore.Data.Context
{
    public class Context<TScope> : Context, IContext<TScope>
    {
        public static IContext<TScope> Current { get; protected set; }
        public TScope Data { get; set; }

        public IResponseChannel Response
            => scoped.GetRequiredService<IResponseChannel>();

        IResponseChannel Channel { get; }

        IServiceProvider scoped;

        public Context(IServiceProvider scopedProvider) 
        {
            scoped = scopedProvider;            
            Current = this;
        }   
    }
}
