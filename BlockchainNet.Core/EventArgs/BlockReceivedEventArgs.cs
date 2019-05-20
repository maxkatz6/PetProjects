namespace BlockchainNet.Core.EventArgs
{
    using System.Linq;
    using System.Collections.Generic;

    using BlockchainNet.Core.Models;
    using BlockchainNet.Shared.EventArgs;

    public class BlockReceivedEventArgs<TInstruction> : DeferredEventArgs
    {
        public BlockReceivedEventArgs(IEnumerable<Block<TInstruction>> addedBlocks, string? clientId)
        {
            AddedBlocks = addedBlocks.ToList();
            ClientId = clientId;
        }

        public IReadOnlyList<Block<TInstruction>> AddedBlocks { get; }

        public string? ClientId { get; }
    }
}
