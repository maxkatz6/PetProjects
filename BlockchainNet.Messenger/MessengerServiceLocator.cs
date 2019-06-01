namespace BlockchainNet.Messenger
{
    using BlockchainNet.Core;
    using BlockchainNet.Core.Consensus;
    using BlockchainNet.Core.Interfaces;
    using BlockchainNet.Core.Models;
    using BlockchainNet.Core.Services;
    using BlockchainNet.IO.TCP;
    using BlockchainNet.LiteDB;
    using BlockchainNet.Messenger.Models;
    using System.Globalization;
    using System.Threading;

    public class MessengerServiceLocator
    {
        public MessengerServiceLocator(int? startPort = null)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            var minPort = startPort ?? TcpHelper.DefaultPort;
            var currentPort = TcpHelper.GetAvailablePort(minPort, minPort + 1000);
            var databaseName = $"database_{currentPort}.litedb";

           // System.IO.File.Delete(databaseName);
            BlockRepository = new LiteDBBlockRepository<MessageInstruction>(databaseName);

            SignatureService = new SignatureService();
            Communicator = new Communicator<MessageInstruction>(
                new LiteDBPeerRepository(databaseName),
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
