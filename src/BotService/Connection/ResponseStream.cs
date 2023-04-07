using BotService.Common;
using MediatR;
using MissBot.Abstractions;
using MissBot.Commands;
using MissCore.Data.Context;

namespace BotService.Connection
{
    internal class ResponseStream : Context, IResponseStream 
    {
        IMediator mediator;
       // Message<BaseEntity.Unit<object>> message;
        public ResponseStream(IMediator mm)
        {

        }

        public Task CloseAsync()
        {
            throw new NotImplementedException();
        }

        //public Task FlushAsync(CancellationToken cancel)
        //{
        //    var items = from value in this select value.Value.ToStringJson();
        //    message.Text = string.Join("\n", items);
        //    return mediator.Send(message.Update(),cancel);
        //}

        public Task OpenAsync()
        {
            //  message = mediator.Send(new GetResponseMessage<Message<object>>());
            return Task.CompletedTask;
        }

        public void Write<T>(T value)
        {
            
        }

        public Task WriteAsync<T>(T value)
        {

            return Task.CompletedTask;
        }
    }
}
