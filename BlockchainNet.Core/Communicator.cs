namespace BlockchainNet.Core
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using BlockchainNet.IO;
    using BlockchainNet.Core.Models;
    using BlockchainNet.Core.Interfaces;
    using BlockchainNet.Shared.EventArgs;
    using BlockchainNet.Core.EventArgs;
    using BlockchainNet.IO.Models;

    public class Communicator<TInstruction> : ICommunicator<TInstruction>
    {
        private readonly ICommunicationServer<BlockchainPayload<TInstruction>> server;
        private readonly ICommunicationClientFactory<BlockchainPayload<TInstruction>> clientFactory;

        private readonly List<ICommunicationClient<BlockchainPayload<TInstruction>>> nodes;

        private readonly IBlockRepository<TInstruction> blockRepository;

        /// <summary>
        /// Конструктор коммуникатора
        /// </summary>
        /// <param name="server">Сервер</param>
        /// <param name="clientFactory">Фаблика клиентов</param>
        public Communicator(
            IBlockRepository<TInstruction> blockRepository,
            ICommunicationServer<BlockchainPayload<TInstruction>> server,
            ICommunicationClientFactory<BlockchainPayload<TInstruction>> clientFactory)
        {
            this.blockRepository = blockRepository;
            this.server = server ?? throw new ArgumentNullException(nameof(server), "Server must be setted");
            this.clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory), "Client factory must be setted");

            nodes = new List<ICommunicationClient<BlockchainPayload<TInstruction>>>();

            server.MessageReceivedEvent += Server_MessageReceivedEvent;
            server.ClientConnectedEvent += Server_ClientConnectedEvent;
            server.ClientDisconnectedEvent += Server_ClientDisconnectedEvent;
        }

        public event EventHandler<BlockReceivedEventArgs<TInstruction>>? BlockReceived;

        public event EventHandler<ClientInformation>? ClientConnected;

        public string? Login { get; set; }

        public string ServerId => server.ServerId;

        public Task StartAsync()
        {
            return server.StartAsync();
        }

        public async Task ConnectToAsync(IEnumerable<string> serversId)
        {
            await server.StartAsync().ConfigureAwait(false);

            foreach (var serverId in serversId)
            {
                var nodeClient = clientFactory.CreateNew(serverId, new ClientInformation(server.ServerId, Login));
                await nodeClient.StartAsync().ConfigureAwait(false);
                nodes.Add(nodeClient);
            }
        }

        public Task CloseAsync()
        {
            return Task.WhenAll(
                new[] { server.StopAsync() }
                .Concat(nodes
                .Select(n => n
                .StopAsync())));
        }

        public Task BroadcastBlocksAsync(
            IEnumerable<Block<TInstruction>> blocks,
            Func<ICommunicationClient<BlockchainPayload<TInstruction>>, bool>? filter = null)
        {
            return Task.WhenAll(nodes
                .Where(filter ?? (peer => true))
                .Select(n => n
                .SendMessageAsync(new BlockchainPayload<TInstruction>
                {
                    Action = Enum.BlockchainPayloadAction.BroadcastBlocks,
                    Blocks = blocks
                })));
        }

        public Task BroadcastRequestAsync(string blockId)
        {
            if (blockId == null)
            {
                throw new ArgumentNullException(nameof(blockId));
            }

            return Task.WhenAll(nodes
                .Select(n => n
                .SendMessageAsync(new BlockchainPayload<TInstruction>
                {
                    Action = Enum.BlockchainPayloadAction.RequestBlocks,
                    BlockId = blockId
                })));
        }

        protected void OnClientConnected(ClientInformation information)
        {
            ClientConnected?.Invoke(this, information);
        }

        private async void Server_ClientConnectedEvent(object sender, ClientConnectedEventArgs e)
        {
            if (e.ClientInformation.ClientId == null)
            {
                return;
            }

            if (nodes.All(c => c.ServerId != e.ClientInformation.ClientId))
            {
                using var _ = e.GetDeferral();

                var nodeClient = clientFactory.CreateNew(e.ClientInformation.ClientId, new ClientInformation(server.ServerId, Login));
                await nodeClient.StartAsync().ConfigureAwait(false);
                nodes.Add(nodeClient);
            }
            OnClientConnected(e.ClientInformation);
        }

        private async void Server_ClientDisconnectedEvent(object sender, ClientDisconnectedEventArgs e)
        {
            using (e.GetDeferral())
            {
                var node = nodes.Find(n => n.ServerId == e.ClientId);
                if (node != null)
                {
                    _ = nodes.Remove(node);
                    await node.StopAsync().ConfigureAwait(false);
                }
            }
        }

        private async void Server_MessageReceivedEvent(object sender, MessageReceivedEventArgs<BlockchainPayload<TInstruction>> e)
        {
            using (e.GetDeferral())
            {
                if (e.Message.Action == Enum.BlockchainPayloadAction.RequestBlocks)
                {
                    if (e.Message.BlockId == null)
                    {
                        throw new ArgumentNullException(nameof(e.Message.BlockId));
                    }

                    var fork = await blockRepository.GetFork(e.Message.BlockId).ToListAsync().ConfigureAwait(false);
                    var nodeClient = nodes.Find(c => c.ServerId == e.ClientId);
                    _ = await nodeClient
                        .SendMessageAsync(new BlockchainPayload<TInstruction>
                        {
                            Action = Enum.BlockchainPayloadAction.ResponseBlocks,
                            BlockId = fork.FirstOrDefault()?.Id,
                            Blocks = fork
                        })
                        .ConfigureAwait(false);
                }
                else
                {
                    await BlockReceived.InvokeAsync(this, new BlockReceivedEventArgs<TInstruction>(e.Message.Blocks, e.ClientId));
                }
            }
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask(CloseAsync());
        }
    }
}
