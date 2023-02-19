using MediatR;
using MissBot.Handlers;

namespace MissDataMaiden
{
    internal class MissDataCommandHandler : BaseCommandHandler<MissDataMaid>
    {
        IMediator mm;
        public MissDataCommandHandler(IMediator mediator)
            => mm = mediator;
        
    }
}
