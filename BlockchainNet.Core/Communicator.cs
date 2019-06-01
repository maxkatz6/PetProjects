namespace BlockchainNet.Core
{
    using System;
    using System.Linq;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Collections.Concurrent;

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

        private readonly ConcurrentDictionary<string, ICommunicationClient<BlockchainPayload<TInstruction>>> activeConnections;

        private readonly IPeerRepository peerRepository;
        private readonly IBlockRepository<TInstruction> blockRepository;

        /// <summary>
        /// Конструктор коммуникатора
        /// </summary>
        /// <param name="server">Сервер</param>
        /// <param name="clientFactory">Фаблика клиентов</param>
        public Communicator(
            IPeerRepository peerRepository,
            IBlockRepository<TInstruction> blockRepository,
            ICommunicationServer<BlockchainPayload<TInstruction>> server,
            ICommunicationClientFactory<BlockchainPayload<TInstruction>> clientFactory)
        {
            this.peerRepository = peerRepository;
            this.blockRepository = blockRepository;
            this.server = server ?? throw new ArgumentNullException(nameof(server), "Server must be setted");
            this.clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory), "Client factory must be setted");

            activeConnections = new ConcurrentDictionary<string, ICommunicationClient<BlockchainPayload<TInstruction>>>();

            server.MessageReceivedEvent += Server_MessageReceivedEvent;
            server.ClientConnectedEvent += Server_ClientConnectedEvent;
            server.ClientDisconnectedEvent += Server_ClientDisconnectedEvent;
        }

        public event EventHandler<BlockReceivedEventArgs<TInstruction>>? BlockReceived;

        public event EventHandler<ClientInformation>? ClientConnected;

        public string? Login { get; set; }

        public string ServerId => server.ServerId;

        public async ValueTask StartAsync()
        {
            await server.StartAsync().ConfigureAwait(false);
        }

        public async Task ConnectToAsync(IEnumerable<string> serversId)
        {
            if (!server.IsListening)
            {
                throw new InvalidOperationException("Server is not started");
            }

            foreach (var serverId in serversId)
            {
                var nodeClient = clientFactory.CreateNew(serverId, new ClientInformation(server.ServerId, Login));
                await nodeClient.StartAsync().ConfigureAwait(false);
                _ = activeConnections.TryAdd(nodeClient.ServerId, nodeClient);
            }
        }

        public ValueTask CloseAsync()
        {
            if (activeConnections.Count == 0)
            {
                return server.StopAsync();
            }

            return new ValueTask(Task.WhenAll(
                new[] { server.StopAsync().AsTask() }
                .Concat(activeConnections
                .Values
                .Select(n => n
                .StopAsync()
                .AsTask()))));
        }

        public Task BroadcastBlocksAsync(
            IEnumerable<Block<TInstruction>> blocks,
            string? channel,
            Func<Peer, bool>? filter = null)
        {
            if (blocks == null)
            {
                throw new ArgumentNullException(nameof(blocks));
            }

            var message = new BlockchainPayload<TInstruction>
            {
                Action = Enum.BlockchainPayloadAction.BroadcastBlocks,
                Blocks = blocks,
                Channel = channel
            };
            return BroadcastMessageAsync(message, channel, filter).AsTask();
        }

        public Task BroadcastRequestAsync(string blockId, string? channel)
        {
            if (blockId == null)
            {
                throw new ArgumentNullException(nameof(blockId));
            }
            var message = new BlockchainPayload<TInstruction>
            {
                Action = Enum.BlockchainPayloadAction.RequestBlocks,
                BlockId = blockId,
                Channel = channel
            };
            return BroadcastMessageAsync(message, channel).AsTask();
        }

        private ValueTask<int> BroadcastMessageAsync(
            BlockchainPayload<TInstruction> payload,
            string? channel,
            Func<Peer, bool>? filter = null)
        {
            return peerRepository
               .GetPeersAsync(channel)
               .OrderBy(p => p.LastMessaged ?? DateTime.MinValue)
               .Where(filter ?? (peer => true))
               .SelectAwait(async p =>
               {
                   try
                   {
                       var connection = activeConnections.GetOrAdd(
                           p.IpEndpoint!,
                           endpoint => clientFactory.CreateNew(endpoint, new ClientInformation(ServerId, Login)));
                       await connection.StartAsync().ConfigureAwait(false);
                       return await connection
                           .SendMessageAsync(payload)
                           .ConfigureAwait(false);
                   }
                   catch (Exception ex)
                   {
                       _ = activeConnections.TryRemove(p.IpEndpoint!, out _);
                       Debug.WriteLine(ex);
                       return false;
                   }
               })
               .Where(res => res)
               .Take(3)
               .CountAsync();
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

            using var _ = e.GetDeferral();

            await peerRepository.UpsertPeerAsync(new Peer
            {
                IpEndpoint = e.ClientInformation.ClientId
            });
            OnClientConnected(e.ClientInformation);
        }

        private async void Server_ClientDisconnectedEvent(object sender, ClientDisconnectedEventArgs e)
        {
            if (activeConnections.TryRemove(e.ClientId, out var client))
            {
                using var _ = e.GetDeferral();
                await client.StopAsync().ConfigureAwait(false);
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
                    var message = new BlockchainPayload<TInstruction>
                    {
                        Action = Enum.BlockchainPayloadAction.ResponseBlocks,
                        BlockId = fork.FirstOrDefault()?.Id,
                        Blocks = fork
                    };
                    var count = await BroadcastMessageAsync(message, e.Message.Channel, peer => peer.IpEndpoint == e.ClientId).ConfigureAwait(false);
                    if (count == 0)
                    {
                        throw new InvalidOperationException("Nothing was sent");
                    }
                }
                else if (e.Message.Blocks is IEnumerable<Block<TInstruction>> blocks)
                {
                    await BlockReceived.InvokeAsync(this, new BlockReceivedEventArgs<TInstruction>(blocks, e.ClientId, e.Message.Channel));
                }
            }
        }
    }
}
