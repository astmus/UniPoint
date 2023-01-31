using Microsoft.Extensions.DependencyInjection;
using MissCore.Abstractions;

namespace MissCore.Data.Context
{
    public class Context<TScope> : Context, IContext<TScope>
    {
        public static IContext<TScope> Current { get; protected set; }
        public string Id { get; }
        public TScope Data { get; }

        IServiceProvider scoped;
        IServiceScope Scope;
        public Context(IServiceProvider scopedProvider) : base(scopedProvider)
        {
            scoped = scopedProvider;
        }

        public Context(IServiceScope scope) : this(scope.ServiceProvider)
        {
            Scope = scope;
        }
        public IContext<TScope> With<T>(T value, string name = null) where T : class
        {
            Set(value, name);
            return this;
        }

        public IContext<T> GetContextOf<T>(string name = null)
            => Get<IContext<T>>();
        public T GetContext<T>(bool createIfNotExists = true, string name = null) where T : class, IHandleContext
            => createIfNotExists ? GetOrCreateNew<T>() : Get<T>();
        T GetOrCreateNew<T>(string name = null) where T : class
        {
            var res = Get<T>() ?? default;
            if (res == default)
            {
                res = ActivatorUtilities.GetServiceOrCreateInstance<T>(scoped);
                Set(res);
            }
            return res;
        }
    }
}
