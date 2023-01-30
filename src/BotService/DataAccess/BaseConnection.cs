using System.Net;
using System.Runtime.CompilerServices;
using BotService.DataAccess.Extensions;
using BotService.Interfaces;
using MissBot;
using MissCore.Configuration;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Requests;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

namespace BotService.DataAccess
{
    /// <summary>
    /// A client to use the Telegram Bot API
    /// </summary>
    public abstract class BaseConnection
    {
        public abstract IBotConnectionOptions Options { get; set; }
        readonly HttpClient _httpClient;

        /// <summary>
        /// Timeout for requests
        /// </summary>
        public TimeSpan Timeout
        {
            get => Options.Timeout;
            set => _httpClient.Timeout = value;
        }

        public BaseConnection(HttpClient httpClient = default)
        {
            //Options = options ?? throw new ArgumentNullException(nameof(options));
            _httpClient = httpClient ?? new HttpClient();
        }
        public virtual async Task<TResponse> MakeRequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {

        //public virtual async Task<TResponse> BotRequestAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IBotRequest<TResponse>
        //{

            /// <inheritdoc />
            if (request is null) { throw new ArgumentNullException(nameof(request)); }

            var url = $"{Options.BaseRequestUrl}/{request.MethodName}";

#pragma warning disable CA2000
            var httpRequest = new HttpRequestMessage(method: request.Method, requestUri: url)
            {
                Content = request.ToHttpContent()
            };
#pragma warning restore CA2000


            var requestEventArgs = new ApiRequestEventArgs(
                request: request,
                httpRequestMessage: httpRequest
            );



            using var httpResponse = await SendRequestAsync(httpClient: _httpClient, httpRequest: httpRequest, cancellationToken: cancellationToken).ConfigureAwait(false);

            requestEventArgs = new ApiRequestEventArgs(
                request: request,
                httpRequestMessage: httpRequest
            );
            var responseEventArgs = new ApiResponseEventArgs(
                responseMessage: httpResponse,
                apiRequestEventArgs: requestEventArgs
            );



            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                var failedApiResponse = await httpResponse
                    .DeserializeContentAsync<ApiResponse>(
                        guard: response =>
                            response.ErrorCode == default ||
                            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                            response.Description is null
                    )
                    .ConfigureAwait(false);

                throw Options.ExceptionsParser.Parse(failedApiResponse);
            }

            var apiResponse = await httpResponse
                .DeserializeContentAsync<ApiResponse<TResponse>>(
                    guard: response => response.Ok == false ||
                                       response.Result is null, Options.SerializeSettings
                )
                .ConfigureAwait(false);

            return apiResponse.Result!;

            [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
            static async Task<HttpResponseMessage> SendRequestAsync(
                HttpClient httpClient,
                HttpRequestMessage httpRequest,
                CancellationToken cancellationToken)
            {
                HttpResponseMessage httpResponse;
                try
                {
                    httpResponse = await httpClient
                        .SendAsync(request: httpRequest, cancellationToken: cancellationToken)
                        .ConfigureAwait(continueOnCapturedContext: false);
                }
                catch (TaskCanceledException exception)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        throw;
                    }

                    throw new RequestException(message: "Request timed out", innerException: exception);
                }
                catch (Exception exception)
                {
                    throw new RequestException(
                        message: "Exception during making request",
                        innerException: exception
                    );
                }

                return httpResponse;
            }
        }

        /// <summary>
        /// Test the API token
        /// </summary>
        /// <returns><c>true</c> if token is valid</returns>
        public async Task<bool> GetBotClientAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await MakeRequestAsync<User>(request: new ParameterlessRequest<User>("getMe"), cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
                return true;
            }
            catch (ApiRequestException e)
                when (e.ErrorCode == 401)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public async Task DownloadFileAsync(
            string filePath,
            Stream destination,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(filePath) || filePath.Length < 2)
            {
                throw new ArgumentException(message: "Invalid file path", paramName: nameof(filePath));
            }

            if (destination is null) { throw new ArgumentNullException(nameof(destination)); }

            var fileUri = $"{Options.BaseFileUrl}/{filePath}";
            using var httpResponse = await GetResponseAsync(
                httpClient: _httpClient,
                fileUri: fileUri,
                cancellationToken: cancellationToken
            ).ConfigureAwait(false);

            if (!httpResponse.IsSuccessStatusCode)
            {
                var failedApiResponse = await httpResponse.DeserializeContentAsync<ApiResponse>(guard: response =>
                            response.ErrorCode == default || response.Description is null
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                    )
                    .ConfigureAwait(false);

                throw Options.ExceptionsParser.Parse(failedApiResponse);
            }

            if (httpResponse.Content is null)
            {
                throw new RequestException(
                    message: "Response doesn't contain any content",
                    httpResponse.StatusCode
                );
            }

            try
            {
                await httpResponse.Content.CopyToAsync(destination).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                throw new RequestException(
                    message: "Exception during file download",
                    httpResponse.StatusCode,
                    exception
                );
            }

            [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
            static async Task<HttpResponseMessage> GetResponseAsync(HttpClient httpClient, string fileUri, CancellationToken cancellationToken)
            {
                HttpResponseMessage httpResponse;
                try
                {
                    httpResponse = await httpClient
                        .GetAsync(requestUri: fileUri, completionOption: HttpCompletionOption.ResponseHeadersRead, cancellationToken: cancellationToken)
                        .ConfigureAwait(continueOnCapturedContext: false);
                }
                catch (TaskCanceledException exception)
                {
                    if (cancellationToken.IsCancellationRequested) { throw; }
                    throw new RequestException(message: "Request timed out", innerException: exception);
                }
                catch (Exception exception)
                {
                    throw new RequestException(message: "Exception during file download", innerException: exception);
                }

                return httpResponse;
            }
        }
    }
}
