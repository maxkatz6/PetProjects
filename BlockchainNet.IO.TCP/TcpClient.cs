namespace BlockchainNet.IO.TCP
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using BlockchainNet.IO;

    using Newtonsoft.Json;

    public class TcpClient<T> : ICommunicationClient<T>
    {
        private Socket? _socket;

        public TcpClient(string serverId, string responseServerId)
        {
            ServerId = serverId ?? throw new ArgumentNullException(nameof(serverId));
            ResponseServerId = responseServerId ?? throw new ArgumentNullException(nameof(responseServerId));
        }

        public string ServerId { get; }

        public string ResponseServerId { get; }

        public async Task StartAsync()
        {
            var parts = ServerId.Split(':');
            if (!int.TryParse(parts.Skip(1).FirstOrDefault(), out var port))
            {
                port = TcpHelper.DefaultPort;
            }

            var ipHost = await Dns.GetHostEntryAsync(IPAddress.Parse(parts.First())).ConfigureAwait(false);
            var address = ipHost.AddressList.FirstOrDefault(adr => adr.AddressFamily == AddressFamily.InterNetwork);

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await _socket
                .ConnectAsync(address, port)
                .ConfigureAwait(false);

            _ = await SendMessageInternalAsync(ResponseServerId);
        }

        public Task StopAsync()
        {
            _socket?.Dispose();
            return Task.CompletedTask;
        }

        public Task<bool> SendMessageAsync(T message)
        {
            return SendMessageInternalAsync(message);
        }

        private async Task<bool> SendMessageInternalAsync<TMsg>(TMsg message)
        {
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
            var length = await _socket.SendAsync(new List<ArraySegment<byte>> { firstSegment, secondSegment }, SocketFlags.None);
            return length > 0;
        }
    }
}
