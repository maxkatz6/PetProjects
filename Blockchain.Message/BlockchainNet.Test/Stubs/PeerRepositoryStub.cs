namespace BlockchainNet.Test.Stubs
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using BlockchainNet.Core.Interfaces;
    using BlockchainNet.Core.Models;

    public class InMemoryPeerRepositoryStub : IPeerRepository
    {
        private readonly ConcurrentDictionary<string, Peer> peers
            = new ConcurrentDictionary<string, Peer>();

        public ValueTask UpsertPeerAsync(Peer peer)
        {
            if (peer.IpEndpoint == null)
            {
                throw new ArgumentNullException(nameof(peer.IpEndpoint));
            }
            _ = peers.AddOrUpdate(peer.IpEndpoint, peer, (id, old) => peer);
            return new ValueTask();
        }

        public IAsyncEnumerable<Peer> GetPeersAsync()
        {
            return peers.Values.ToAsyncEnumerable();
        }

        public ValueTask RemovePeerByIpAsync(string ip)
        {
            peers.TryRemove(ip, out _);
            return new ValueTask();
        }
    }
}
