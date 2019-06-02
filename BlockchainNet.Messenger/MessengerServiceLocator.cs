namespace BlockchainNet.Messenger
{
    using System.Threading;
    using System.Globalization;

    using BlockchainNet.Core;
    using BlockchainNet.Core.Consensus;
    using BlockchainNet.Core.Interfaces;
    using BlockchainNet.Core.Models;
    using BlockchainNet.Core.Services;
    using BlockchainNet.IO.TCP;
    using BlockchainNet.LiteDB;
    using BlockchainNet.Messenger.Models;
    using System;
    using System.Text;

    public class MessengerServiceLocator
    {
        public MessengerServiceLocator(int? startPort = null)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.OutputEncoding = Console.InputEncoding = Encoding.GetEncoding(1251);

            var minPort = startPort ?? TcpHelper.DefaultPort;
            var currentPort = TcpHelper.GetAvailablePort(minPort, minPort + 1000);
            var databaseName = $"database_{currentPort}.litedb";

            SignatureService = new SignatureService();
            Communicator = new Communicator<MessageInstruction>(
                new LiteDBPeerRepository(databaseName),
                new TcpServer<BlockchainPayload<MessageInstruction>>(currentPort),
                new TcpClientFactory<BlockchainPayload<MessageInstruction>>());
            Channels = new MessengerChannels(
                databaseName,
                Communicator,
                new ProofOfWorkConsensus<MessageInstruction>(),
                SignatureService);
        }

        public ISignatureService SignatureService { get; }

        public Communicator<MessageInstruction> Communicator { get; }

        public MessengerChannels Channels { get; }
    }
}
