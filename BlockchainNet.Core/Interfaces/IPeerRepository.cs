namespace BlockchainNet.Core.Interfaces
{
    using BlockchainNet.Core.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPeerRepository
    {
        IAsyncEnumerable<Peer> GetPeersAsync(string? channel);

        ValueTask UpsertPeerAsync(Peer peer);

        ValueTask RemovePeerByIpAsync(string ip);
    }
}
