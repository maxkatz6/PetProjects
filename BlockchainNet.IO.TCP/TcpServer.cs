namespace BlockchainNet.IO.TCP
{
    using System;
    using System.Text;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using BlockchainNet.IO;

    using ProtoBuf;
    using System.Linq;

    public class TcpServer<T> : ICommunicationServer<T>
    {
        private readonly Socket _socket;
        private bool _isStopping;
        private int _port;

        private class Package
        {
            public byte[] Buffer = new byte[2048];
            public List<byte> Result = new List<byte>();
        }

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
            var address = ipHost.AddressList.FirstOrDefault(adr => adr.AddressFamily == AddressFamily.InterNetwork);

            _socket.Bind(new IPEndPoint(address, _port));
            ServerId = _socket.LocalEndPoint.ToString();

            _socket.Listen(20);

            _ = Task.Run(async () =>
            {
                while (true)
                {
                    var client = await _socket.AcceptAsync().ConfigureAwait(false);

                    var buffer = new byte[2048];
                    var length = client.Receive(buffer);
                    responceClientId = Encoding.UTF8.GetString(buffer, 0, length);

                    if (responceClientId == "\0")
                    {
                        responceClientId = null;
                    }

                    ClientConnectedEvent?.Invoke(
                        this,
                        new ClientConnectedEventArgs { ClientId = responceClientId });

                    _ = Task.Run(() => ReadAsync(client, new Package()));
                }
            });
        }

        public Task StopAsync()
        {
            _isStopping = true;

            _socket.Dispose();

            return Task.CompletedTask;
        }

        private async Task ReadAsync(Socket client, Package package)
        {
            // побуферно читаем сообщения, что позволяет не ограничиваться его размером
            var arraySegment = new ArraySegment<byte>(package.Buffer);
            var readBytes = await client.ReceiveAsync(arraySegment, SocketFlags.None).ConfigureAwait(false);

            if (readBytes > 0)
            {
                byte[] result;
                if (readBytes == package.Buffer.Length)
                {
                    result = package.Buffer;
                }
                else
                {
                    result = new byte[readBytes];
                    Array.Copy(package.Buffer, 0, result, 0, readBytes);
                }

                package.Result.AddRange(result);

                // если не дочитали...
                if (readBytes == package.Buffer.Length)
                {
                    // ...то читаем следующий буфер
                    await ReadAsync(client, package);
                }
                else
                {
                    using (var stream = new MemoryStream(package.Result.ToArray()))
                    {
                        var message = Serializer.Deserialize<T>(stream);

                        MessageReceivedEvent?.Invoke(this,
                            new MessageReceivedEventArgs<T>
                            {
                                ClientId = responceClientId,
                                Message = message
                            });
                    }
                    await ReadAsync(client, new Package());
                }
            }
            // Если прочитано 0 байт, то клиент вероятно отключился
            else
            {
                if (!_isStopping)
                {
                    ClientDisconnectedEvent?.Invoke(
                        this,
                        new ClientDisconnectedEventArgs { ClientId = responceClientId });
                }
            }
        }
    }
}
