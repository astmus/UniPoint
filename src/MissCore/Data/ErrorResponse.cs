using System.Web;
using MissBot.Abstractions;
using MissBot.Entities;
using MissCore.Collections;

namespace MissCore.Data
{

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record ErrorResponse(IHandleContext Context = default) : Response<Unit>(Context), IResponseError
    {
        Message Message
            => Context.Take<Message>();        

        public IResponseError Write(Exception error)
        {            
            var stack = HttpUtility.HtmlEncode(error.StackTrace);
           // Content = error;//$"{error.Message}\n{error.InnerException?.Message}\n{stack}";
            return this;
        }
    }
}


