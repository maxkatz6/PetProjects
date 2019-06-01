namespace BlockchainNet.LiteDB
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using BlockchainNet.Core.Interfaces;
    using BlockchainNet.Core.Models;
    
    using global::LiteDB;

    public class LiteDBPeerRepository : IPeerRepository
    {
        private readonly LiteCollection<Peer> chain;

        public LiteDBPeerRepository(string fileName)
        {
            var database = new LiteDatabase(new ConnectionString { Filename = fileName });
            chain = database.GetCollection<Peer>("peers");
            _ = BsonMapper.Global
                .Entity<Peer>()
                .Id(b => b.IpEndpoint);
            
            _ = chain.EnsureIndex(x => x.IpEndpoint, true);
        }

        public IAsyncEnumerable<Peer> GetPeersAsync(string? channel)
        {
            return chain.Find(p => p.Channel == channel && p.IpEndpoint != null).ToAsyncEnumerable();
        }

        public ValueTask RemovePeerByIpAsync(string ip)
        {
            _ = chain.Delete(ip);
            return new ValueTask();
        }

        public ValueTask UpsertPeerAsync(Peer peer)
        {
            _ = chain.Upsert(peer);
            return new ValueTask();
        }
    }
}
