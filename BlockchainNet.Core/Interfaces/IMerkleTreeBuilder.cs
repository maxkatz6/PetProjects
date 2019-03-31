namespace BlockchainNet.Core.Interfaces
{
    using System.Collections.Generic;

    using BlockchainNet.Core.Models;

    public interface IMerkleTreeBuilder
    {
        MerkleNode BuildTree(IEnumerable<byte[]> nodes);
    }
}
