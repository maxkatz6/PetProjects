namespace BlockchainNet.IO.TCP
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using BlockchainNet.IO;
    using BlockchainNet.Shared.EventArgs;

    using Newtonsoft.Json;

    public class TcpServer<T> : ICommunicationServer<T>
    {
        private const int HeaderSize = sizeof(long);

        private readonly Socket _socket;
        private bool _isStopping;
        private readonly int _port;

        public TcpServer()
            : this(TcpHelper.GetAvailablePort())
        {

        }

        public TcpServer(int port)
        {
            _port = port;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public event EventHandler<ClientConnectedEventArgs> ClientConnectedEvent;
        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnectedEvent;
        public event EventHandler<MessageReceivedEventArgs<T>> MessageReceivedEvent;

        public string ServerId => _socket?.LocalEndPoint?.ToString()
            ?? throw new InvalidOperationException("TcpServer is not started");

        public async Task StartAsync()
        {
            _isStopping = false;

            var ipHost = await Dns.GetHostEntryAsync("localhost").ConfigureAwait(false);
            var address = Array.Find(ipHost.AddressList, adr => adr.AddressFamily == AddressFamily.InterNetwork);

            _socket.Bind(new IPEndPoint(address, _port));

            _socket.Listen(20);

            _ = Task.Run(async () =>
            {
                while (!_isStopping)
                {
                    Socket client;
                    try
                    {
                        client = await _socket.AcceptAsync().ConfigureAwait(false);
                    }
                    catch
                    {
                        await StopAsync();
                        throw;
                    }

                    var responseClientId = await ReadInternalAsync<string?>(client, null).ConfigureAwait(false);
                    if (string.IsNullOrEmpty(responseClientId))
                    {
                        throw new InvalidOperationException("Client returned invalid response id");
                    }

                    await ClientConnectedEvent
                        .InvokeAsync(this, new ClientConnectedEventArgs(responseClientId))
                        .ConfigureAwait(false);

                    _ = Task.Run(() => ReadLoopAsync(client, responseClientId));
                }
            });
        }

        public Task StopAsync()
        {
            _isStopping = true;

            _socket.Dispose();

            return Task.CompletedTask;
        }

        private async Task ReadLoopAsync(Socket client, string responseClientId)
        {
            while (true)
            {
                var message = await ReadInternalAsync<T>(client, responseClientId).ConfigureAwait(false);
                if (message == null)
                {
                    return;
                }

                await MessageReceivedEvent
                    .InvokeAsync(this, new MessageReceivedEventArgs<T>(responseClientId, message))
                    .ConfigureAwait(false);
            }
        }

        private async Task<TMsg> ReadInternalAsync<TMsg>(Socket client, string? responseClientId)
        {
            if (!client.Connected)
            {
                return default!;
            }
            var headerSegment = new ArraySegment<byte>(new byte[HeaderSize], 0, HeaderSize);
            var headerLength = await client.ReceiveAsync(headerSegment, SocketFlags.None).ConfigureAwait(false);
            if (headerLength == 0)
            {
                await OnDisconnectedAsync(responseClientId).ConfigureAwait(false);
                return default!;
            }

            if (headerLength != HeaderSize)
            {
                throw new IndexOutOfRangeException();
            }

            var length = BitConverter.ToInt32(headerSegment.Array, headerSegment.Offset);
            var dataArray = new byte[length];
            var offset = 0;
            do
            {
                if (!client.Connected)
                {
                    return default!;
                }
                var dataSegment = new ArraySegment<byte>(dataArray, offset, length - offset);
                var dataLength = await client.ReceiveAsync(dataSegment, SocketFlags.None).ConfigureAwait(false);
                if (dataLength == 0)
                {
                    await OnDisconnectedAsync(responseClientId).ConfigureAwait(false);
                    return default!;
                }
                offset += dataLength;
            }
            while (offset < length);

            using var stream = new MemoryStream(dataArray, 0, dataArray.Length);
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);

            var serializer = JsonSerializer.CreateDefault();

            return serializer.Deserialize<TMsg>(jsonReader);

            async Task OnDisconnectedAsync(string? responceClientIdIn)
            {
                if (!_isStopping)
                {
                    if (responceClientIdIn == null)
                    {
                        throw new ArgumentNullException(nameof(responceClientIdIn));
                    }

                    await ClientDisconnectedEvent
                        .InvokeAsync(this, new ClientDisconnectedEventArgs(responceClientIdIn))
                        .ConfigureAwait(false);
                }
            }
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask(StopAsync());
        }
    }
}
