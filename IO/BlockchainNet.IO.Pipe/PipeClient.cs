namespace BlockchainNet.IO.Pipe
{
    using System;
    using System.IO;
    using System.IO.Pipes;
    using System.Text;
    using System.Threading.Tasks;

    using BlockchainNet.IO;
    using BlockchainNet.IO.Models;

    using ProtoBuf;

    public class PipeClient<T> : ICommunicationClient<T>
    {
        private NamedPipeClientStream? _pipeClient;

        public PipeClient(string serverId, ClientInformation responseClient)
        {
            ServerId = serverId ?? throw new ArgumentNullException(nameof(serverId));
            ResponseClient = responseClient ?? throw new ArgumentNullException(nameof(responseClient));
        }

        public string ServerId { get; }

        public ClientInformation ResponseClient { get; }

        public async Task StartAsync()
        {
            if (_pipeClient == null)
            {
                _pipeClient = new NamedPipeClientStream(
                    ".",
                    ServerId,
                    PipeDirection.Out,
                    PipeOptions.Asynchronous);
            }

            await _pipeClient.ConnectAsync().ConfigureAwait(false);

            // Сразу отправляем Id сервера для обратной связи
            var buffer = Encoding.UTF8.GetBytes(ResponseClient.ClientId);
            await _pipeClient.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
        }

        public Task StopAsync()
        {
            if (_pipeClient == null)
            {
            return Task.CompletedTask;
            }

            try
            {
                _pipeClient.WaitForPipeDrain();
            }
            finally
            {
                _pipeClient.Close();
                _pipeClient.Dispose();
                _pipeClient = null;
            }
            return Task.CompletedTask;
        }

        public async Task<bool> SendMessageAsync(T message)
        {
            if (_pipeClient == null)
            {
                return false;
            }

            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, message);

                await _pipeClient.WriteAsync(stream.ToArray(), 0, (int)stream.Length);
                await _pipeClient.FlushAsync();
            }
            return true;
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask(StopAsync());
        }
    }
}
