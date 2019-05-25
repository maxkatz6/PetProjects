namespace BlockchainNet.IO.Pipe
{
    using System;
    using System.IO;
    using System.IO.Pipes;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using BlockchainNet.IO;
    using BlockchainNet.IO.Models;
    using BlockchainNet.Shared.EventArgs;

    using Newtonsoft.Json;

    internal class InternalPipeServer<T> : ICommunicationServer<T>
    {
        private readonly NamedPipeServerStream _pipeServer;
        private bool _isStopping;

        private class Package
        {
            public byte[] Buffer = new byte[PipeServer<T>.BufferSize];
            public List<byte> Result = new List<byte>();
        }

        public InternalPipeServer(string pipeName, int maxNumberOfServerInstances)
        {
            _pipeServer = new NamedPipeServerStream(
                pipeName,
                PipeDirection.In,
                maxNumberOfServerInstances,
                PipeTransmissionMode.Message,
                PipeOptions.Asynchronous);

            ServerId = pipeName;
        }

        public event EventHandler<ClientConnectedEventArgs> ClientConnectedEvent;
        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnectedEvent;
        public event EventHandler<MessageReceivedEventArgs<T>> MessageReceivedEvent;

        public string ServerId { get; }

        public Task StartAsync()
        {
            _ = _pipeServer.WaitForConnectionAsync().ContinueWith(async _ =>
            {
                if (!_isStopping)
                {
                    var buffer = new byte[2048];
                    var length = await _pipeServer.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                    var message = DeserializeBuffer<ClientInformation>(buffer, length);
                    if (message?.ClientId == null)
                    {
                        throw new InvalidOperationException("Client returned invalid response id");
                    }

                    await ClientConnectedEvent
                        .InvokeAsync(this, new ClientConnectedEventArgs(message))
                        .ConfigureAwait(false);

                    await ReadAsync(new Package(), message.ClientId).ConfigureAwait(false);
                }
            });
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            _isStopping = true;

            try
            {
                if (_pipeServer.IsConnected)
                {
                    _pipeServer.Disconnect();
                }
            }
            finally
            {
                _pipeServer.Close();
                _pipeServer.Dispose();
            }
            return Task.CompletedTask;
        }

        private async Task ReadAsync(Package package, string responseId)
        {
            // побуферно читаем сообщения, что позволяет не ограничиваться его размером

            var readBytes = await _pipeServer.ReadAsync(package.Buffer, 0, package.Buffer.Length);

            if (readBytes > 0)
            {
                byte[] result;
                if (_pipeServer.IsMessageComplete)
                {
                    result = new byte[readBytes];
                    Array.Copy(package.Buffer, 0, result, 0, readBytes);
                }
                else
                {
                    result = package.Buffer;
                }

                package.Result.AddRange(result);

                // если не дочитали...
                if (!_pipeServer.IsMessageComplete)
                {
                    // ...то читаем следующий буфер
                    await ReadAsync(package, responseId);
                }
                else
                {
                    var message = DeserializeBuffer<T>(package.Result.ToArray());
                    await MessageReceivedEvent
                        .InvokeAsync(this, new MessageReceivedEventArgs<T>(responseId, message))
                        .ConfigureAwait(false);

                    await ReadAsync(new Package(), responseId);
                }
            }
            // Если прочитано 0 байт, то клиент вероятно отключился
            else
            {
                if (!_isStopping)
                {
                    await StopAsync().ConfigureAwait(false);
                    await ClientDisconnectedEvent
                        .InvokeAsync(this, new ClientDisconnectedEventArgs(responseId))
                        .ConfigureAwait(false);
                }
            }
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask(StopAsync());
        }

        private static TMsg DeserializeBuffer<TMsg>(byte[] buffer, int? length = null)
        {
            using var stream = new MemoryStream(buffer, 0, length ?? buffer.Length);
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);

            var serializer = JsonSerializer.CreateDefault();

            return serializer.Deserialize<TMsg>(jsonReader);
        }
    }
}
