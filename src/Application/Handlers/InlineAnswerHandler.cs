using MissBot.Abstractions;
using MissBot.Entities.Results;
using MissCore.Handlers;

namespace MissBot.Handlers
{

    public abstract class InlineAnswerHandler : BaseHandler<ChosenInlineResult>
    {

        public async override Task HandleAsync(ChosenInlineResult data, IHandleContext context)
        {

            //var response = context.CreateResponse(data);

            await HandleResultAsync(data, context);
            //if (items.Count() != 0)
            //    response.WriteResult(items);
            //else
            //    response.Write(InlineUnit.Empty);
            // await response.Commit(default);

        }
        public virtual int? BatchSize { get; }
        public abstract Task HandleResultAsync(ChosenInlineResult result, IHandleContext context);
        //{
        //    return Task.FromResult(new BotUnion<InlineUnit>() { InlineUnit.Empty } as BotUnion);
        //}

    }
}
