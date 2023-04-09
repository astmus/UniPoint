using MissBot.Abstractions;

namespace MissCore.Data.Context
{
    public class Context<TScope> : Context, IContext<TScope>
    {
        public Context()
            => Current = this;

        public TScope Data
        {
            get => Get<TScope>();
            set => Set(value);
        }

        public IBotServicesProvider BotServices { get; set; }

        IServiceProvider scoped;

        public Context(IServiceProvider scopedProvider)
        {
            scoped = scopedProvider;
            Current = this;
        }


        public IHandleContext SetupData<T>(IContext context, T data)
        {
            context.Set(data);
            return this;
        }

        public T NextHandler<T>() where T : class
            => BotServices.GetService<T>();

        public IResponse Response
        {
            get
            {
                var res = BotServices.GetRequiredService<IResponse>();
                res.SetContext(this);
                return res;
            }
        }
    }
}
