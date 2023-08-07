using System;
using System.Web;
using MissBot.Abstractions;
using MissBot.Entities;
using MissCore.Data;
using System.Data;
using Newtonsoft.Json.Linq;

namespace MissCore.Response
{
	[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public record ErrorResponse(IHandleContext Context = default) : Response<Exception>(Context), IResponseError
	{
		Message Message
			=> Context.Take<Message>();

		public IResponseError Write(Exception error)
		{
			HttpUtility.HtmlEncode(error.StackTrace);
			var v = DataUnit<Exception>.Init(error);

			Content = v;
			return this;
		}
	}
}


