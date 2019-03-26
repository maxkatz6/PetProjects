namespace BlockchainNet.IO.TCP
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using BlockchainNet.IO;

    using ProtoBuf;

    public class TcpServer<T> : ICommunicationServer<T>
    {
        private const int HeaderSize = sizeof(long);

        private readonly Socket _socket;
        private bool _isStopping;
        private int _port;

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

        public string ServerId { get; private set; }

        private string responceClientId;

        public async Task StartAsync()
        {
            var ipHost = await Dns.GetHostEntryAsync("localhost").ConfigureAwait(false);
            var address = Array.Find(ipHost.AddressList, adr => adr.AddressFamily == AddressFamily.InterNetwork);

            _socket.Bind(new IPEndPoint(address, _port));
            ServerId = _socket.LocalEndPoint.ToString();

            _socket.Listen(20);

            _ = Task.Run(async () =>
            {
                while (true)
                {
                    var client = await _socket.AcceptAsync().ConfigureAwait(false);

                    var responceClientId = await ReadInternalAsync<string>(client).ConfigureAwait(false);
                    if (responceClientId == "\0")
                    {
                        responceClientId = null;
                    }

                    ClientConnectedEvent?.Invoke(
                        this,
                        new ClientConnectedEventArgs { ClientId = responceClientId });

                    _ = Task.Run(() => ReadLoopAsync(client));
                }
            });
        }

        public Task StopAsync()
        {
            _isStopping = true;

            _socket.Dispose();

            return Task.CompletedTask;
        }

        private async Task ReadLoopAsync(Socket client)
        {
            while (true)
            {
                var message = await ReadInternalAsync<T>(client).ConfigureAwait(false);
                if (message == null)
                {
                    return;
                }

                MessageReceivedEvent?.Invoke(this,
                    new MessageReceivedEventArgs<T>
                    {
                        ClientId = responceClientId,
                        Message = message
                    });
            }
        }

        private async Task<TMsg> ReadInternalAsync<TMsg>(Socket client)
        {
            if (!client.Connected)
            {
                return default;
            }
            var headerSegment = new ArraySegment<byte>(new byte[HeaderSize], 0, HeaderSize);
            var headerLength = await client.ReceiveAsync(headerSegment, SocketFlags.None).ConfigureAwait(false);
            if (headerLength == 0)
            {
                if (!_isStopping)
                {
                    ClientDisconnectedEvent?.Invoke(
                        this,
                        new ClientDisconnectedEventArgs { ClientId = responceClientId });
                }
                return default;
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
                    return default;
                }
                var dataSegment = new ArraySegment<byte>(dataArray, offset, length - offset);
                var dataLength = await client.ReceiveAsync(dataSegment, SocketFlags.None).ConfigureAwait(false);
                if (dataLength == 0)
                {
                    if (!_isStopping)
                    {
                        ClientDisconnectedEvent?.Invoke(
                            this,
                            new ClientDisconnectedEventArgs { ClientId = responceClientId });
                    }
                    return default;
                }
                offset += dataLength;
            }
            while (offset < length);

            using (var stream = new MemoryStream(dataArray, 0, dataArray.Length))
            {
                return Serializer.Deserialize<TMsg>(stream);
            }
        }
    }
}
