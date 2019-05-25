namespace BlockchainNet.Test
{
    using System.Threading.Tasks;

    using BlockchainNet.Core.Consensus;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using BlockchainNet.Core.Services;
    using BlockchainNet.Messenger;
    using BlockchainNet.Test.Stubs;
    using BlockchainNet.Messenger.Models;
    using BlockchainNet.Core.Interfaces;
    using Moq;
    using BlockchainNet.Core.Models;
    using System.Collections.Generic;
    using System;
    using BlockchainNet.IO;

    [TestClass]
    public class BlockchainTest
    {
        [TestMethod]
        public async Task Blockchain_MineTest()
        {
            var communicatorMock = new Mock<ICommunicator<MessageInstruction>>();
            _ = communicatorMock.SetupGet(c => c.Login).Returns("Alice");
            _ = communicatorMock.Setup(c => c.BroadcastBlocksAsync(
                    It.IsAny<IEnumerable<Block<MessageInstruction>>>(),
                    It.IsAny<Func<ICommunicationClient<BlockchainPayload<MessageInstruction>>, bool>?>()))
                .Returns(Task.CompletedTask);
            var signatureService = new SignatureService();
            var repository = new InMemoryBlockRepository<MessageInstruction>();
            var consensus = new ProofOfWorkConsensus<MessageInstruction>();
            var blockchain = new MessengerBlockchain(communicatorMock.Object, repository, consensus, signatureService);

            var keys = signatureService.GetKeysFromPassword("password");
            blockchain.NewTransaction("Alice", "Bob", new MessageInstruction("Hello world"), keys);

            var minedBlock = await blockchain.MineAsync("Alice", default).ConfigureAwait(false);
            var isValid = blockchain.IsValidChain(new[] { minedBlock });

            Assert.IsTrue(isValid, "Blockchain is invalid");
        }
    }
}
