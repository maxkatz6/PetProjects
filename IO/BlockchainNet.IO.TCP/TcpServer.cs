namespace BlockchainNet.IO.TCP
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using BlockchainNet.IO;
    using BlockchainNet.IO.Models;
    using BlockchainNet.Shared.EventArgs;

    using Newtonsoft.Json;
    using Open.Nat;

    public class TcpServer<T> : ICommunicationServer<T>
    {
        private const int HeaderSize = sizeof(long);

        private readonly Socket _socket;
        private readonly int _port;
        private bool _isStopping;
        private string? serverEndpoint;

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

        public string ServerId => serverEndpoint ?? throw new InvalidOperationException("Server is not started");

        public bool IsListening { get; private set; }

        public async ValueTask StartAsync()
        {
            if (IsListening)
            {
                return;
            }

            _isStopping = false;

            _socket.Bind(new IPEndPoint(IPAddress.Any, _port));

            _socket.Listen(20);
            IsListening = true;

            var ip = await GetDeviceIpAsync().ConfigureAwait(false);
            serverEndpoint = ip + ":" + _port;

            _ = ConnectLoopAsync();

            async Task ConnectLoopAsync()
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

                    var clientInformation = await ReadInternalAsync<ClientInformation?>(client, null).ConfigureAwait(false);
                    if (string.IsNullOrEmpty(clientInformation?.ClientId))
                    {
                        throw new InvalidOperationException("Client returned invalid response id");
                    }

                    await ClientConnectedEvent
                        .InvokeAsync(this, new ClientConnectedEventArgs(clientInformation))
                        .ConfigureAwait(false);

                    _ = ReadLoopAsync(client, clientInformation.ClientId);
                }
            }
        }

        public ValueTask StopAsync()
        {
            _isStopping = true;
            IsListening = false;

            _socket.Dispose();

            return new ValueTask();
        }

        private async Task<string?> GetDeviceIpAsync()
        {
            try
            {
                var discoverer = new NatDiscoverer();
                var device = await discoverer.DiscoverDeviceAsync().ConfigureAwait(false);
                var ip = await device.GetExternalIPAsync().ConfigureAwait(false);
                return ip?.MapToIPv4().ToString();
            }
            catch (Exception)
            {
                var ipHost = await Dns.GetHostEntryAsync("localhost").ConfigureAwait(false);
                return Array.Find(ipHost.AddressList, adr => adr.AddressFamily == AddressFamily.InterNetwork)?
                    .MapToIPv4()
                    .ToString();
            }
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
            try
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
            }
            catch (SocketException exception)
            when (exception.SocketErrorCode == SocketError.ConnectionReset)
            {
                await OnDisconnectedAsync(responseClientId).ConfigureAwait(false);
                return default!;
            }

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
    }
}
