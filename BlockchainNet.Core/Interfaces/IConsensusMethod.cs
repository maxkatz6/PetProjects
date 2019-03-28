namespace BlockchainNet.Core.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;

    using BlockchainNet.Core.Models;

    public interface IConsensusMethod<TContent>
    {
        Task BuildConsensus(Block<TContent> block, CancellationToken cancellationToken);
        bool VerifyConsensus(Block<TContent> block);
    }
}
