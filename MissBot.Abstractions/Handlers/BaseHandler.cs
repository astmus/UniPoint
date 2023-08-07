namespace MissBot.Abstractions.Handlers
{
	public abstract class BaseHandler<TData> : IAsyncHandler<TData> where TData : class
	{
		public virtual AsyncHandler AsyncDelegate
			=> HandleAsync;
		Lazy<IResponse<TData>> _response;

		public IResponse<TData> Response
			=> _response.Value;

		public IHandleContext Context { get; private set; }
		async Task HandleAsync(IHandleContext context)
		{
			if (context.Get<TData>() is TData data)
			{
				SetContext(context);
				await HandleAsync(data).ConfigureAwait(false);
			}
			else
				await context.GetNextHandler().ConfigureAwait(false);

			if (context.IsHandled.HasValue)
				return;
		}

		public void SetContext(IHandleContext context)
		{
			Context = context;
			_response = new Lazy<IResponse<TData>>(()
				=> Context.Response<TData>(), true);
		}

		public abstract Task HandleAsync(TData data, CancellationToken cancel = default);
	}
}
