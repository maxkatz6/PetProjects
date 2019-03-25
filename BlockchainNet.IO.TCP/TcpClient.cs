namespace BlockchainNet.IO.TCP
{
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    using BlockchainNet.IO;

    using ProtoBuf;

    public class TcpClient<T> : ICommunicationClient<T>
    {
        private Socket _socket;

        public TcpClient(string serverId)
        {
            ServerId = serverId;
        }

        public string ServerId { get; }

        public string ResponseServerId { get; set; }

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

            // Сразу отправляем Id сервера для обратной связи
            var buffer = Encoding.UTF8.GetBytes(string.IsNullOrEmpty(ResponseServerId) ? "\0" : ResponseServerId);
            _socket.Send(buffer);
        }

        public Task StopAsync()
        {
            _socket.Dispose();
            return Task.CompletedTask;
        }

        public Task<bool> SendMessageAsync(T message)
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, message);
                _socket.Send(stream.ToArray());
            }
            return Task.FromResult(true);
        }
    }
}
