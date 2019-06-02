namespace BlockchainNet.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BlockchainNet.Core.Consensus;
    using BlockchainNet.Core.Services;
    using BlockchainNet.Messenger;
    using BlockchainNet.Test.Stubs;
    using BlockchainNet.Messenger.Models;
    using BlockchainNet.Core.Interfaces;
    using BlockchainNet.Core.Models;

    using Moq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                    It.IsAny<string>(),
                    It.IsAny<Func<Peer, bool>?>()))
                .Returns(Task.FromResult(1));
            var signatureService = new SignatureService();
            var repository = new InMemoryBlockRepository<MessageInstruction>();
            var consensus = new ProofOfWorkConsensus<MessageInstruction>();
            var blockchain = new MessengerBlockchain(communicatorMock.Object, repository, consensus, signatureService);

            var keys = signatureService.GetKeysFromPassword("password");
            await blockchain.NewTransactionAsync("Alice", "Bob", new MessageInstruction("Hello world"), keys);

            var minedBlock = await blockchain.MineAsync(default).ConfigureAwait(false);
            var isValid = await blockchain.IsValidChainAsync(new[] { minedBlock }).ConfigureAwait(false);

            Assert.IsTrue(isValid, "Blockchain is invalid");
        }

        [TestMethod]
        public async Task Blockchain_InvalidPasswordWithHackTest()
        {
            var communicatorMock = new Mock<ICommunicator<MessageInstruction>>();
            _ = communicatorMock.SetupGet(c => c.Login).Returns("Alice");
            _ = communicatorMock.Setup(c => c.BroadcastBlocksAsync(
                    It.IsAny<IEnumerable<Block<MessageInstruction>>>(),
                    It.IsAny<string>(),
                    It.IsAny<Func<Peer, bool>?>()))
                .Returns(Task.FromResult(1));
            var signatureService = new SignatureService();
            var repository = new InMemoryBlockRepository<MessageInstruction>();
            var consensus = new ProofOfWorkConsensus<MessageInstruction>();
            var blockchain = new MessengerBlockchain(communicatorMock.Object, repository, consensus, signatureService);

            var keys = signatureService.GetKeysFromPassword("password");
            await blockchain.NewTransactionAsync("Alice", "Bob", new MessageInstruction("Hello world"), keys);
            _ = await blockchain.MineAsync(default).ConfigureAwait(false);
            
            var invalidKeys = signatureService.GetKeysFromPassword("drowssap");
            var transaction = new Transaction<MessageInstruction>("Alice", "Bob", invalidKeys.publicKey, new MessageInstruction(""), DateTime.Now);
            signatureService.SignTransaction(transaction, invalidKeys.privateKey);
            ((List<Transaction<MessageInstruction>>)blockchain.CurrentTransactions).Add(transaction);

            var minedBlock = await blockchain.MineAsync(default).ConfigureAwait(false);
            var isValid = await blockchain.IsValidChainAsync(new[] { minedBlock }).ConfigureAwait(false);

            Assert.IsFalse(isValid, "Blockchain should be invalid");
        }

        [TestMethod]
        public async Task Blockchain_InvalidPasswordTest()
        {
            var communicatorMock = new Mock<ICommunicator<MessageInstruction>>();
            _ = communicatorMock.SetupGet(c => c.Login).Returns("Alice");
            _ = communicatorMock.Setup(c => c.BroadcastBlocksAsync(
                    It.IsAny<IEnumerable<Block<MessageInstruction>>>(),
                    It.IsAny<string>(),
                    It.IsAny<Func<Peer, bool>?>()))
                .Returns(Task.FromResult(1));
            var signatureService = new SignatureService();
            var repository = new InMemoryBlockRepository<MessageInstruction>();
            var consensus = new ProofOfWorkConsensus<MessageInstruction>();
            var blockchain = new MessengerBlockchain(communicatorMock.Object, repository, consensus, signatureService);

            var keys = signatureService.GetKeysFromPassword("password");
            await blockchain.NewTransactionAsync("Alice", "Bob", new MessageInstruction("Hello world"), keys);
            _ = await blockchain.MineAsync(default).ConfigureAwait(false);
            
            keys = signatureService.GetKeysFromPassword("password2");
            _ = Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => blockchain.NewTransactionAsync("Alice", "Bob", new MessageInstruction("Hello world"), keys));
        }
    }
}
