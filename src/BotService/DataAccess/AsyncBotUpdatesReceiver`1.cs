#if NETCOREAPP3_1_OR_GREATER
using System.Diagnostics;
using System.Threading.Channels;
using BotService.Internal;
using MissCore.Abstractions;
using MissCore.Configuration;
using MissCore.DataAccess.Async;
using Telegram.Bot.Types.Enums;

namespace BotService.DataAccess
{

    /// <summary>
    /// Supports asynchronous iteration over <see cref="Update"/>s.
    /// <para>Updates are received on a different thread and enqueued.</para>
    /// </summary>

    public class AsyncBotUpdatesReceiver<TUpdate> : IBotUpdatesReceiver<TUpdate> where TUpdate : class, IUpdateInfo
    {
        readonly IBotConnection botConnection;
        //readonly ReceiverOptions _receiverOptions;
        //readonly Func<Exception, CancellationToken, Task> _pollingErrorHandler;
        protected IBotConnectionOptions Options
            => botConnection.Options;

        int _inProcess;
        Enumerator _enumerator;

        /// <summary>
        /// Constructs a new <see cref="QueuedUpdateReceiver"/> for the specified <see cref="IBotConnection"/>
        /// </summary>
        /// <param name="connection">The <see cref="IBotConnection"/> used for making GetUpdates calls</param>
        /// <param name="receiverOptions"></param>
        /// <param name="pollingErrorHandler">
        /// The function used to handle <see cref="Exception"/>s thrown by GetUpdates requests
        /// </param>
        public AsyncBotUpdatesReceiver(IBotConnection connection)
            => botConnection = connection ?? throw new ArgumentNullException(nameof(connection));


        /// <summary>
        /// Indicates how many <see cref="Update"/>s are ready to be returned the enumerator
        /// </summary>
        public int PendingUpdates => _enumerator?.PendingUpdates ?? 0;

        /// <summary>
        /// Gets the <see cref="IAsyncEnumerator{Update}"/>. This method may only be called once.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> with which you can stop receiving
        /// </param>
        public IAsyncEnumerator<TUpdate> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            if (Interlocked.CompareExchange(ref _inProcess, 1, 0) == 1)
            {
                throw new InvalidOperationException(nameof(GetAsyncEnumerator) + " may only be called once");
            }

            _enumerator = new(receiver: this, cancellationToken: cancellationToken);

            return _enumerator;
        }

        class Enumerator : IAsyncEnumerator<TUpdate>
        {
            readonly AsyncBotUpdatesReceiver<TUpdate> _receiver;
            readonly CancellationTokenSource _cts;
            readonly CancellationToken _token;
            readonly UpdateType[] _allowedUpdates;
            readonly sbyte _limit = 1;

            Exception? _uncaughtException;

            readonly Channel<TUpdate> _channel;
            TUpdate _current;

            int _pendingUpdates;
            uint _messageOffset;
            GetUpdatesRequest<TUpdate> getUpdatesRequest;
            public int PendingUpdates => _pendingUpdates;

            public Enumerator(AsyncBotUpdatesReceiver<TUpdate> receiver, CancellationToken cancellationToken)
            {
                _receiver = receiver;
                _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, default);
                _token = _cts.Token;
                _messageOffset = receiver.Options?.Offset ?? 0;
                _limit = receiver.Options?.Limit ?? default;
                _allowedUpdates = receiver.Options?.AllowedUpdates;

                _channel = Channel.CreateUnbounded<TUpdate>(
                    new()
                    {
                        SingleReader = true,
                        SingleWriter = true
                    }
                );
                getUpdatesRequest = new GetUpdatesRequest<TUpdate>
                {
                    Offset = _messageOffset,
                    Timeout = _receiver.botConnection.Timeout,
                    AllowedUpdates = _allowedUpdates,
                    Limit = _limit,
                };

                Task.Run(ReceiveUpdatesAsync, _cts.Token);
            }

            public ValueTask<bool> MoveNextAsync()
            {
                if (_uncaughtException is not null) { throw _uncaughtException; }

                _token.ThrowIfCancellationRequested();

                if (_channel.Reader.TryRead(out _current))
                {
                    Interlocked.Decrement(ref _pendingUpdates);
                    return new(true);
                }

                return new(ReadAsync());
            }

            async Task<bool> ReadAsync()
            {
                _current = await _channel.Reader.ReadAsync(_token).ConfigureAwait(false);
                Interlocked.Decrement(ref _pendingUpdates);
                return true;
            }

            async Task ReceiveUpdatesAsync()
            {
                //if (_receiver._receiverOptions?.ThrowPendingUpdates is true)
                //{
                //    try
                //    {
                //        _messageOffset = await _receiver._botClient.ThrowOutPendingUpdatesAsync(
                //            cancellationToken: _token
                //        ).ConfigureAwait(false);
                //    }
                //    catch (OperationCanceledException)
                //    {
                //        // ignored
                //    }
                //}

                while (!_cts.IsCancellationRequested)
                {
                    try
                    {
                        TUpdate[] updateArray = await _receiver.botConnection.MakeRequestAsync(
                                request: getUpdatesRequest with { Offset = _messageOffset },
                                cancellationToken: _token
                            ).ConfigureAwait(false);

                        if (updateArray.Length > 0)
                        {
                            _messageOffset = updateArray[^1].UpdateId + 1;

                            Interlocked.Add(ref _pendingUpdates, updateArray.Length);

                            var writer = _channel.Writer;
                            foreach (var update in updateArray)
                            {
                                // ReSharper disable once RedundantAssignment
                                var success = writer.TryWrite(update);
                                Debug.Assert(success, "TryWrite should succeed as we are using an unbounded channel");
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // Ignore
                    }
#pragma warning disable CA1031
                    catch (Exception ex)
#pragma warning restore CA1031
                    {
                        Debug.Assert(_uncaughtException is null);

                        // If there is no errorHandler or the errorHandler throws, stop receiving
                        if (_receiver.Options.ConnectionErrorHandler is null)
                        {
                            _uncaughtException = ex;
                            _cts.Cancel();
                        }
                        else
                        {
                            try
                            {
                                await _receiver.Options.ConnectionErrorHandler(ex, _token).ConfigureAwait(false);
                            }
#pragma warning disable CA1031
                            catch (Exception errorHandlerException)
#pragma warning restore CA1031
                            {
                                _uncaughtException = new AggregateException(
                                    message: "Exception was not caught by the errorHandler.",
                                    ex,
                                    errorHandlerException
                                );
                                _cts.Cancel();
                            }
                        }

                        if (_uncaughtException is not null)
                        {
#pragma warning disable CA2201
                            _uncaughtException = new(
                                message: "Exception was not caught by the errorHandler.",
                                innerException: _uncaughtException
                            );
#pragma warning restore CA2201
                        }
                    }
                }
            }

            public TUpdate Current => _current!; // _current being null indicates MoveNextAsync was never called

            public ValueTask DisposeAsync()
            {
                _cts.Cancel();
                _cts.Dispose();
                return new();
            }
        }
    }
#endif
}
