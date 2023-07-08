using System.Text;
using MissBot.Entities;

namespace MissBot.Abstractions
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract record BaseRequest<TResponse> : IBotRequest<TResponse>
    {
        protected virtual JsonConverter CustomConverter { get; }
        /// <inheritdoc />
        [JsonIgnore]
        public HttpMethod Method { get; }

        /// <inheritdoc />
        [JsonIgnore]
        public string MethodName { get; protected set; }

        /// <summary>
        /// Initializes an instance of request
        /// </summary>
        /// <param name="methodName">Bot API method</param>
        protected BaseRequest(string methodName)
            : this(methodName, HttpMethod.Post) { }

        /// <summary>
        /// Initializes an instance of request
        /// </summary>
        /// <param name="methodName">Bot API method</param>
        /// <param name="method">HTTP method to use</param>
        protected BaseRequest(string methodName, HttpMethod method)
        {
            MethodName = methodName;
            Method = method;
        }

        /// <summary>
        /// Generate content of HTTP message
        /// </summary>
        /// <returns>Content of HTTP request</returns>
        public virtual HttpContent ToHttpContent()
        {
            var payload = CustomConverter is JsonConverter converter ? JsonConvert.SerializeObject(this, converter) : JsonConvert.SerializeObject(this);
            return new StringContent(payload, Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Allows this object to be used as a response in webhooks
        /// </summary>
        [JsonIgnore]
        public bool IsWebhookResponse { get; set; }

        /// <summary>
        /// If <see cref="IsWebhookResponse"/> is set to <see langword="true"/> is set to the method
        /// name, otherwise it won't be serialized
        /// </summary>

        [JsonProperty("method", DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal string WebHookMethodName => IsWebhookResponse ? MethodName : default;
    }

    public record BaseParameterlessRequest<TResponse> : BaseRequest<TResponse>
    {
        /// <summary>
        /// Initializes an instance of <see cref="ParameterlessRequest{TResult}"/>
        /// </summary>
        /// <param name="methodName">Name of request method</param>
        public BaseParameterlessRequest(string methodName)
            : base(methodName)
        {
        }

        /// <summary>
        /// Initializes an instance of <see cref="ParameterlessRequest{TResult}"/>
        /// </summary>
        /// <param name="methodName">Name of request method</param>
        /// <param name="method">HTTP request method</param>
        public BaseParameterlessRequest(string methodName, HttpMethod method)
            : base(methodName, method)
        {
        }

        /// <inheritdoc cref="RequestBase{TResponse}.ToHttpContent"/>
        public override HttpContent ToHttpContent() => IsWebhookResponse
            ? base.ToHttpContent()
            : null;
    }

}
