namespace BlockchainNet.Core.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;

    using BlockchainNet.Core.Models;

    public interface IConsensusMethod<TInstruction>
    {
        Task BuildConsensus(Block<TInstruction> block, CancellationToken cancellationToken);
        bool VerifyConsensus(Block<TInstruction> block);
    }
}
