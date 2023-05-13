using MissBot.Abstractions;
using MissBot.Extensions;
using MissCore;
using MissCore.Handlers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

namespace MissBot.Handlers
{ 
    public record InlineUnit : Unit<InlineQuery>
    {
        private const string empty = "Empty";        
        public new string Id { get => Get<string>(); set => Set(value); }
        public string Title { get => Get<string>(); set => Set(value); }
        //public virtual object Content
        //        => $"{Id}\n{Title}\n{Description}"; 
        public string Description { get => Get<string>(); set => Set(value); }

        public static readonly InlineUnit Empty
            = new InlineUnit() { Id = "0", Title = "Not found"};
    }

    public record InlineUnit<TEntity> : InlineUnit
    {
        private const string empty = "Empty";
        public new string Id { get => Get<string>(); set => Set(value); }
        public TEntity Content { get; set; }
    }


    public abstract class InlineQueryHandler : BaseHandler<InlineQuery>
    {       

        public async override Task HandleAsync(InlineQuery data, IHandleContext context)
        {
    
           // var response = context.CreateResponse(data);

            await LoadAsync(data.Offset.IsNullOrEmpty() ? 0 : int.Parse(data.Offset), data.Query, null, data);
            
            //await response.Commit(default);

        }
        public virtual int? BatchSize { get; }
        
        public abstract Task LoadAsync(int skip, string filter, IResponse<InlineQuery> response, InlineQuery query);
    }
}
