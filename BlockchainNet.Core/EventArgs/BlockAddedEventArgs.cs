namespace BlockchainNet.Core.EventArgs
{
    using System.Linq;
    using System.Collections.Generic;

    using BlockchainNet.Core.Models;
    using BlockchainNet.Shared.EventArgs;

    public class BlockAddedEventArgs<TInstruction> : DeferredEventArgs
    {
        public BlockAddedEventArgs(IEnumerable<Block<TInstruction>> addedBlocks)
        {
            AddedBlocks = addedBlocks.ToList();
        }

        public IReadOnlyList<Block<TInstruction>> AddedBlocks { get; }
    }
}
