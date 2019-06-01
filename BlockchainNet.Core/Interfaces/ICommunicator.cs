﻿namespace BlockchainNet.Core.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using BlockchainNet.Core.EventArgs;
    using BlockchainNet.Core.Models;
    using BlockchainNet.IO;
    using BlockchainNet.IO.Models;

    public interface ICommunicator<TInstruction> : IAsyncDisposable
    {
        event EventHandler<BlockReceivedEventArgs<TInstruction>>? BlockReceived;

        event EventHandler<ClientInformation>? ClientConnected;

        string? Login { get; set; }

        string ServerId { get; }

        ValueTask StartAsync();

        Task ConnectToAsync(IEnumerable<string> serversId);

        ValueTask CloseAsync();

        Task BroadcastBlocksAsync(
            IEnumerable<Block<TInstruction>> blocks,
            string? channel,
            Func<Peer, bool>? filter = null);

        Task BroadcastRequestAsync(string blockId, string? channel);

        ValueTask IAsyncDisposable.DisposeAsync()
        {
            return CloseAsync();
        }
    }
}
