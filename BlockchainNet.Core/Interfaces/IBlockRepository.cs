namespace BlockchainNet.Core.Interfaces
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using BlockchainNet.Core.Models;

    public interface IBlockRepository<TInstruction>
    {
        ValueTask AddBlock(Block<TInstruction> block);

        ValueTask<bool> IsEmpty();

        ValueTask<Block<TInstruction>?> GetTopBlock();

        ValueTask<Block<TInstruction>?> GetBlock(string blockId);

        ValueTask RewindChain(string blockId);

        IAsyncEnumerable<Block<TInstruction>> GetFork(string forkTipBlockId);  
    }
}
