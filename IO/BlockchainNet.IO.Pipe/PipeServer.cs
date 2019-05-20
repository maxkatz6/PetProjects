namespace BlockchainNet.IO.Pipe
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.ComponentModel;
    using System.Collections.Concurrent;

    using BlockchainNet.IO;
    using BlockchainNet.Shared.EventArgs;

    public class PipeServer<T> : ICommunicationServer<T>
    {
        public const int BufferSize = 2048;
        public const int MaxNumberOfServerInstances = 100;

        private readonly SynchronizationContext _synchronizationContext;
        private readonly ConcurrentDictionary<string, ICommunicationServer<T>> _servers;

        public PipeServer(string id)
        {
            ServerId = id;
            _synchronizationContext = AsyncOperationManager.SynchronizationContext;
            _servers = new ConcurrentDictionary<string, ICommunicationServer<T>>();
        }

        public PipeServer() : this(Guid.NewGuid().ToString())
        {
        }

        public event EventHandler<MessageReceivedEventArgs<T>> MessageReceivedEvent;
        public event EventHandler<ClientConnectedEventArgs> ClientConnectedEvent;
        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnectedEvent;

        public string ServerId { get; }

        public Task StartAsync()
        {
            return StartNamedPipeServer();
        }

        public async Task StopAsync()
        {
            foreach (var server in _servers.Values)
            {
                try
                {
                    UnregisterFromServerEvents(server);
                    await server.StopAsync();
                }
                catch (Exception)
                {
                }
            }

            _servers.Clear();
        }

        private Task StartNamedPipeServer()
        {
            // Есть смысл создавать свой InternalPipeServer для каждого клиента, но при этом основываясь на одном его имени
            // Это позволяет независимо читать данные с множества клиентов
            var server = new InternalPipeServer<T>(ServerId, MaxNumberOfServerInstances);

            server.ClientConnectedEvent += ClientConnectedHandler;
            server.ClientDisconnectedEvent += ClientDisconnectedHandler;
            server.MessageReceivedEvent += MessageReceivedHandler;

            _servers[server.ServerId] = server;

            return server.StartAsync();
        }

        private Task StopNamedPipeServer(string id)
        {
            if (id != null && _servers.TryRemove(id, out var removed))
            {
                UnregisterFromServerEvents(removed);
                return removed.StopAsync();
            }
            return Task.CompletedTask;
        }

        private void UnregisterFromServerEvents(ICommunicationServer<T> server)
        {
            server.ClientConnectedEvent -= ClientConnectedHandler;
            server.ClientDisconnectedEvent -= ClientDisconnectedHandler;
            server.MessageReceivedEvent -= MessageReceivedHandler;
        }

        private async void ClientConnectedHandler(object sender, ClientConnectedEventArgs eventArgs)
        {
            using (eventArgs.GetDeferral())
            {
                SynchronizationContext.SetSynchronizationContext(_synchronizationContext);
                await ClientConnectedEvent.InvokeAsync(this, eventArgs).ConfigureAwait(false);

                await StartNamedPipeServer().ConfigureAwait(false);
            }
        }

        private async void ClientDisconnectedHandler(object sender, ClientDisconnectedEventArgs eventArgs)
        {
            using (eventArgs.GetDeferral())
            {
                SynchronizationContext.SetSynchronizationContext(_synchronizationContext);
                await ClientDisconnectedEvent.InvokeAsync(this, eventArgs).ConfigureAwait(false);

                await StopNamedPipeServer(eventArgs.ClientId).ConfigureAwait(false);
            }
        }

        private async void MessageReceivedHandler(object sender, MessageReceivedEventArgs<T> eventArgs)
        {
            using (eventArgs.GetDeferral())
            {
                SynchronizationContext.SetSynchronizationContext(_synchronizationContext);
                await MessageReceivedEvent.InvokeAsync(this, eventArgs).ConfigureAwait(false);

                await StopNamedPipeServer(eventArgs.ClientId).ConfigureAwait(false);
            }
        }
    }
}
