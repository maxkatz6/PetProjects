namespace BlockchainNet.IO.TCP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    using BlockchainNet.IO;
    using BlockchainNet.IO.Models;

    using Newtonsoft.Json;

    public class TcpClient<T> : ICommunicationClient<T>
    {
        private Socket? _socket;
        private Timer? timer;
        private IPEndPoint? endpoint;
        private readonly SemaphoreSlim semaphoreSlim;

        public TcpClient(string serverId, ClientInformation responseClient)
        {
            ServerId = serverId ?? throw new ArgumentNullException(nameof(serverId));
            ResponseClient = responseClient ?? throw new ArgumentNullException(nameof(responseClient));
            semaphoreSlim = new SemaphoreSlim(1);
        }

        public string ServerId { get; }

        public ClientInformation ResponseClient { get; }

        public async ValueTask StartAsync()
        {
            var parts = ServerId.Split(':');
            if (!int.TryParse(parts.Skip(1).FirstOrDefault(), out var port))
            {
                port = TcpHelper.DefaultPort;
            }

            var address = IPAddress.Parse(parts.First());
            endpoint = new IPEndPoint(address, port);

            _ = await SendMessageInternalAsync(ResponseClient);
        }

        public ValueTask StopAsync()
        {
            var socket = _socket;
            _socket = null;
            socket?.Dispose();
            return new ValueTask();
        }

        public ValueTask<bool> SendMessageAsync(T message)
        {
            return SendMessageInternalAsync(message);
        }

        private ValueTask EnsureConnect()
        {
            return _socket != null ? new ValueTask() : ConnectAsync();

            async ValueTask ConnectAsync()
            {
                await semaphoreSlim.WaitAsync();
                try
                {
                    if (_socket == null)
                    {
                        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        await _socket
                            .ConnectAsync(endpoint)
                            .ConfigureAwait(false);
                    }
                }
                finally
                {
                    _ = semaphoreSlim.Release();
                }
            }
        }

        private async ValueTask<bool> SendMessageInternalAsync<TMsg>(TMsg message)
        {
            await EnsureConnect();

            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream);
            using var jsonWriter = new JsonTextWriter(writer);

            var serializer = JsonSerializer.CreateDefault();
            serializer.Serialize(jsonWriter, message);
            await jsonWriter.FlushAsync().ConfigureAwait(false);
            _ = stream.Seek(0, SeekOrigin.Begin);

            if (stream.Length == 0)
            {
                throw new InvalidOperationException("Attempt to send empty message");
            }

            var firstSegment = new ArraySegment<byte>(BitConverter.GetBytes(stream.Length), 0, sizeof(long));
            var secondSegment = new ArraySegment<byte>(stream.ToArray());
            var length = await _socket
                .SendAsync(new List<ArraySegment<byte>> { firstSegment, secondSegment }, SocketFlags.None)
                .ConfigureAwait(false);

            CreateTimerForSocket();

            return length > 0;
        }

        private void CreateTimerForSocket()
        {
            _ = timer?.Change(TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));
            _ = timer?.DisposeAsync();
            timer = new Timer(TimerCallback, this, TimeSpan.FromMinutes(1), TimeSpan.FromMilliseconds(-1));
        }

        private void TimerCallback(object state)
        {
            _ = ((TcpClient<T>)state).StopAsync();
        }
    }
}
