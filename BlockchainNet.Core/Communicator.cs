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

    public class Communicator<TInstruction>
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

        public EventHandler<BlockReceivedEventArgs<TInstruction>>? BlockReceived;

        public string ServerId => server.ServerId;

        public Task StartAsync()
        {
            return server.StartAsync();
        }

        public async Task ConnectToAsync(IEnumerable<string> serversId)
        {
            foreach (var serverId in serversId)
            {
                var nodeClient = clientFactory.CreateNew(serverId, server.ServerId);
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

        private async void Server_ClientConnectedEvent(object sender, ClientConnectedEventArgs e)
        {
            using (e.GetDeferral())
            {
                if (nodes.All(c => c.ServerId != e.ClientId)
                    && e.ClientId != null)
                {
                    var nodeClient = clientFactory.CreateNew(e.ClientId, server.ServerId);
                    await nodeClient.StartAsync().ConfigureAwait(false);
                    nodes.Add(nodeClient);
                }
            }
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
    }
}
