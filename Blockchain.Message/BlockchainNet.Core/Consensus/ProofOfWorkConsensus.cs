namespace BlockchainNet.Core.Consensus
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Security.Cryptography;

    using BlockchainNet.Core.Enum;
    using BlockchainNet.Core.Models;
    using BlockchainNet.Core.Interfaces;

    public class ProofOfWorkConsensus<TInstruction> : IConsensusMethod<TInstruction>
    {
        private const int Difficulty = 700;

        public bool VerifyConsensus(Block<TInstruction> block)
        {
            return TestHash(Convert.FromBase64String(block.Id), Difficulty);
        }

        public Task BuildConsensus(Block<TInstruction> block, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var proof = 0L;

                while (!VerifyForNonce(block, proof) && !cancellationToken.IsCancellationRequested)
                {
                    proof++;
                }
            });
        }

        private bool VerifyForNonce(Block<TInstruction> block, long proof)
        {
            var hash = ComputeHash(block.GetHash(proof));

            if (TestHash(hash, Difficulty)
                && block.Status == BlockStatus.Unconfirmed)
            {
                block.ConfirmBlock(Convert.ToBase64String(hash), proof);
                return true;
            }
            return false;
        }

        private bool TestHash(byte[] hash, uint difficulty)
        {
            var counter = difficulty;

            foreach (var b in hash)
            {
                var byteCounter = Math.Min(counter, 255);

                if (b > (255 - byteCounter))
                    return false;

                counter -= byteCounter;

                if (counter <= 0)
                    break;
            }

            return true;
        }

        private byte[] ComputeHash(byte[] input)
        {
            using (var h = SHA256.Create())
            {
                return h.ComputeHash(input);
            }
        }
    }
}
