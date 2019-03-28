namespace BlockchainNet.Test
{
    using System.Threading.Tasks;

    using BlockchainNet.Wallet;
    using BlockchainNet.Core.Consensus;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BlockchainTest
    {
        [TestMethod]
        public async Task Blockchain_MineTest()
        {
            var blockchain = new WalletBlockchain(new ProofOfWorkConsensus<decimal>());
            blockchain.NewTransaction("Alice", "Bob", 0M);
            await blockchain.MineAsync("Alice", default).ConfigureAwait(false);
            var isValid = blockchain.IsValidChain(blockchain.Chain);

            Assert.IsTrue(isValid, "Blockchain is invalid");
        }
    }
}
