namespace BlockchainNet.IO.Pipe
{
    using System;
    using System.IO;
    using System.IO.Pipes;
    using System.Threading.Tasks;

    using BlockchainNet.IO;
    using BlockchainNet.IO.Models;

    using Newtonsoft.Json;

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

            _ = await WriteMessageAsync(ResponseClient).ConfigureAwait(false);
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

        public Task<bool> SendMessageAsync(T message)
        {
            return WriteMessageAsync(message);
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask(StopAsync());
        }

        private async Task<bool> WriteMessageAsync<TMsg>(TMsg message)
        {
            if (_pipeClient == null)
            {
                return false;
            }

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

            await _pipeClient.WriteAsync(stream.ToArray(), 0, (int)stream.Length).ConfigureAwait(false);
            await _pipeClient.FlushAsync().ConfigureAwait(false);
            return true;
        }
    }
}
