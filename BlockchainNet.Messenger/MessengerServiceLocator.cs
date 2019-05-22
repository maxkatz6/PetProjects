namespace BlockchainNet.Messenger
{
    using BlockchainNet.Core;
    using BlockchainNet.Core.Consensus;
    using BlockchainNet.Core.Interfaces;
    using BlockchainNet.Core.Models;
    using BlockchainNet.Core.Services;
    using BlockchainNet.IO.TCP;
    using BlockchainNet.Messenger.Models;

    public class MessengerServiceLocator
    {
        public MessengerServiceLocator()
        {
            var currentPort = TcpHelper.GetAvailablePort();
            System.IO.File.Delete($"database_{currentPort}.litedb");
            BlockRepository = new DefaultBlockRepository<MessageInstruction>($"database_{currentPort}.litedb");

            SignatureService = new SignatureService();
            Communicator = new Communicator<MessageInstruction>(
                BlockRepository,
                new TcpServer<BlockchainPayload<MessageInstruction>>(currentPort),
                new TcpClientFactory<BlockchainPayload<MessageInstruction>>());

            Blockchain = new MessengerBlockchain(
                Communicator,
                BlockRepository,
                new ProofOfWorkConsensus<MessageInstruction>(),
                SignatureService);
        }

        public IBlockRepository<MessageInstruction> BlockRepository { get; }

        public ISignatureService SignatureService { get; }

        public Communicator<MessageInstruction> Communicator { get; }

        public MessengerBlockchain Blockchain { get; }
    }
}
