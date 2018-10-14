namespace BlockchainNet.Test
{
    using BlockchainNet.Wallet;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BlockchainTest
    {
        [TestMethod]
        public void Blockchain_MineTest()
        {
            var blockchain = WalletBlockchain.CreateNew();
            blockchain.NewTransaction("Alice", "Bob", 0);
            var block = blockchain.Mine("Alice");
            var isValid = blockchain.IsValidChain(blockchain.Chain);

            Assert.IsTrue(isValid, "Blockchain is invalid");
        }
    }
}
