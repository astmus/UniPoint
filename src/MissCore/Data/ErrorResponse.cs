using System.Web;
using MissBot.Abstractions;
using MissBot.Entities;

namespace MissCore.Data
{

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record ErrorResponse(IHandleContext Context = default) : Response<Exception>(Context), IErrorResponse
    {
        Message Message
            => Context.Take<Message>();        

        public IErrorResponse Write(Exception error)
        {            
            var stack = HttpUtility.HtmlEncode(error.StackTrace);
            Content = $"{error.Message}\n{error.InnerException?.Message}\n{stack}";
            return this;
        }
    }
}


