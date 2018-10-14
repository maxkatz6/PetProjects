namespace BlockchainNet.Core
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using BlockchainNet.Core.Models;

    public class BlockAddedEventArgs<TContent> : EventArgs
    {
        public Block<TContent> AddedBlock { get; }
        public IReadOnlyList<Block<TContent>> Chain { get; }

        public BlockAddedEventArgs(
            Block<TContent> addedBlock,
            IEnumerable<Block<TContent>> chain = null)
        {
            AddedBlock = addedBlock;
            Chain = chain?.ToList() ?? new List<Block<TContent>> { addedBlock };
        }
    }
}
