namespace BlockchainNet.LiteDB
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using BlockchainNet.Core.Interfaces;
    using BlockchainNet.Core.Models;

    using global::LiteDB;

    public class LiteDBBlockRepository<TInstruction> : IBlockRepository<TInstruction>
    {
        private readonly LiteCollection<Block<TInstruction>> chain;

        public LiteDBBlockRepository(string fileName, string? channel)
        {
            Channel = channel;

            var database = new LiteDatabase(new ConnectionString { Filename = fileName });
            chain = database.GetCollection<Block<TInstruction>>(channel != null ? "blocks_" + channel : "blocks");
            _ = BsonMapper.Global
                .Entity<Block<TInstruction>>()
                .Id(b => b.Id);

            _ = chain.EnsureIndex(x => x.Id, true);
            _ = chain.EnsureIndex(x => x.PreviousBlockId, true);
            _ = chain.EnsureIndex(x => x.Height, true);
        }

        public string? Channel { get; }

        public ValueTask AddBlock(Block<TInstruction> block)
        {
            _ = chain.Insert(block);
            return new ValueTask();
        }

        public ValueTask<bool> IsEmpty()
        {
            return new ValueTask<bool>(!chain.Exists(b => true));
        }

        public ValueTask<Block<TInstruction>?> GetTopBlock()
        {
            if (!chain.Exists(b => true))
            {
                return new ValueTask<Block<TInstruction>?>((Block<TInstruction>?)null);
            }

            var max = chain.Max<uint>(x => x.Height).AsInt64;
            var block = chain.Find(b => b.Height == max).FirstOrDefault();
            return new ValueTask<Block<TInstruction>?>(block);
        }

        public ValueTask<Block<TInstruction>?> GetBlock(string blockId)
        {
            return new ValueTask<Block<TInstruction>?>(chain.FindById(blockId));
        }

        public ValueTask RewindChain(string blockId)
        {
            var newTopBlock = chain.FindById(blockId);
            if (newTopBlock == null)
            {
                return new ValueTask();
            }

            _ = chain.Delete(b => b.Height > newTopBlock.Height);
            return new ValueTask();
        }

        public IAsyncEnumerable<Block<TInstruction>> GetFork(string forkTipBlockId)
        {
            var forkRoot = chain.FindById(forkTipBlockId);
            return forkRoot == null
                ? AsyncEnumerable.Empty<Block<TInstruction>>()
                : chain.Find(b => b.Height >= forkRoot.Height).ToAsyncEnumerable();
        }
    }
}
