namespace BlockchainNet.Core.Interfaces
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using BlockchainNet.Core.Models;

    public interface IBlockRepository<TInstruction>
    {
        string? Channel { get; }

        ValueTask AddBlock(Block<TInstruction> block);

        ValueTask<bool> IsEmpty();

        ValueTask<Block<TInstruction>?> GetTopBlock();

        ValueTask<Block<TInstruction>?> GetBlock(string blockId);

        ValueTask RewindChain(string blockId);

        ValueTask<Transaction<TInstruction>?> GetLastTransactionAsync(string sender);

        IAsyncEnumerable<Block<TInstruction>> GetFork(string forkTipBlockId);
    }
}
