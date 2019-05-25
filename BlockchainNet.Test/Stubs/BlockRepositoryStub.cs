namespace BlockchainNet.Test.Stubs
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using BlockchainNet.Core.Interfaces;
    using BlockchainNet.Core.Models;

    public class InMemoryBlockRepository<TInstruction> : IBlockRepository<TInstruction>
    {
        private readonly ConcurrentDictionary<string, Block<TInstruction>> blocks
            = new ConcurrentDictionary<string, Block<TInstruction>>();

        public ValueTask AddBlock(Block<TInstruction> block)
        {
            if (block.Id == null)
            {
                throw new ArgumentNullException(nameof(block.Id));
            }

            if (!blocks.TryAdd(block.Id, block))
            {
                throw new InvalidOperationException($"Block with Id={block.Id} already added");
            }

            return new ValueTask();
        }

        public ValueTask<Block<TInstruction>?> GetBlock(string blockId)
        {
            if (blockId == null)
            {
                throw new ArgumentNullException(nameof(blockId));
            }

            return new ValueTask<Block<TInstruction>?>(blocks.TryGetValue(blockId, out var block) ? block : null);
        }

        public IAsyncEnumerable<Block<TInstruction>> GetFork(string forkTipBlockId)
        {
            return GetForkSync(forkTipBlockId).ToAsyncEnumerable();

            IEnumerable<Block<TInstruction>> GetForkSync(string forkTipBlockIdIn)
            {
                if (!blocks.TryGetValue(forkTipBlockIdIn, out var block))
                {
                    yield break;
                }

                foreach (var nextBlock in blocks.Values.Where(b => b.Height >= block.Height))
                {
                    yield return nextBlock;
                }
            }
        }

        public ValueTask<Block<TInstruction>?> GetTopBlock()
        {
            return new ValueTask<Block<TInstruction>?>(blocks.Values
                .OrderByDescending(b => b.Height)
                .FirstOrDefault());
        }

        public ValueTask<bool> IsEmpty()
        {
            return new ValueTask<bool>(blocks.Count == 0);
        }

        public ValueTask RewindChain(string blockId)
        {
            if (!blocks.TryGetValue(blockId, out var block))
            {
                return new ValueTask();
            }

            foreach (var nextBlock in blocks.Values.Where(b => b.Height > block.Height))
            {
                _ = blocks.TryRemove(nextBlock.Id!, out _);
            }
            return new ValueTask();
        }
    }
}
