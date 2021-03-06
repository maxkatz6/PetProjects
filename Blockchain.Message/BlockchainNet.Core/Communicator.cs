﻿namespace BlockchainNet.Core
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

        /// <summary>
        /// Конструктор коммуникатора
        /// </summary>
        /// <param name="server">Сервер</param>
        /// <param name="clientFactory">Фаблика клиентов</param>
        public Communicator(
            IPeerRepository peerRepository,
            ICommunicationServer<BlockchainPayload<TInstruction>> server,
            ICommunicationClientFactory<BlockchainPayload<TInstruction>> clientFactory)
        {
            this.peerRepository = peerRepository;
            this.server = server ?? throw new ArgumentNullException(nameof(server), "Server must be setted");
            this.clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory), "Client factory must be setted");

            activeConnections = new ConcurrentDictionary<string, ICommunicationClient<BlockchainPayload<TInstruction>>>();

            server.MessageReceivedEvent += Server_MessageReceivedEvent;
            server.ClientConnectedEvent += Server_ClientConnectedEvent;
            server.ClientDisconnectedEvent += Server_ClientDisconnectedEvent;
        }

        public event EventHandler<BlockReceivedEventArgs<TInstruction>>? BlockReceived;

        public event EventHandler<BlockRequestedEventArgs>? BlockRequested;

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

        public Task<int> BroadcastBlocksAsync(
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
            return BroadcastMessageAsync(message, filter).AsTask();
        }

        public Task<int> BroadcastRequestAsync(string blockId, string? channel)
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
            return BroadcastMessageAsync(message).AsTask();
        }

        private ValueTask<int> BroadcastMessageAsync(
            BlockchainPayload<TInstruction> payload,
            Func<Peer, bool>? filter = null)
        {
            return peerRepository
                .GetPeersAsync()
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
                        var result = await connection
                            .SendMessageAsync(payload)
                            .ConfigureAwait(false);
                        p.LastMessaged = DateTime.UtcNow;
                        await peerRepository.UpsertPeerAsync(p);
                        return result;
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

            await peerRepository.UpsertPeerAsync(new Peer
            {
                IpEndpoint = e.ClientInformation.ClientId
            });

            var newClient = clientFactory.CreateNew(e.ClientInformation.ClientId, new ClientInformation(server.ServerId, Login));
            var added = activeConnections.TryAdd(
                e.ClientInformation.ClientId,
                newClient);
            if (added)
            {
                await newClient.StartAsync().ConfigureAwait(false);
            }
            
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
            using var _ = e.GetDeferral();

            if (e.Message.Action == Enum.BlockchainPayloadAction.RequestBlocks)
            {
                if (e.Message.BlockId == null)
                {
                    throw new ArgumentNullException(nameof(e.Message.BlockId));
                }

                await BlockRequested
                    .InvokeAsync(this, new BlockRequestedEventArgs(e.ClientId, e.Message.BlockId, e.Message.Channel))
                    .ConfigureAwait(false);
            }
            else if (e.Message.Blocks is IEnumerable<Block<TInstruction>> blocks)
            {
                await BlockReceived.InvokeAsync(this, new BlockReceivedEventArgs<TInstruction>(blocks, e.ClientId, e.Message.Channel));
            }
        }
    }
}
