namespace BlockchainNet.Test
{
    using System.Threading.Tasks;

    using BlockchainNet.Core.Consensus;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using BlockchainNet.Core.Services;
    using BlockchainNet.Messenger;

    [TestClass]
    public class BlockchainTest
    {
        [TestMethod]
        public async Task Blockchain_MineTest()
        {
            //var signatureService = new SignatureService();
            //var keys = signatureService.GetKeysFromPassword("password");

            //var blockchain = new MessengerBlockchain(new ProofOfWorkConsensus<decimal>(), signatureService);
            //blockchain.NewTransaction("Alice", "Bob", 0M, keys);

            //await blockchain.MineAsync("Alice", default).ConfigureAwait(false);
            //var isValid = blockchain.IsValidChain(blockchain.Chain);

            //Assert.IsTrue(isValid, "Blockchain is invalid");
        }
    }
}
