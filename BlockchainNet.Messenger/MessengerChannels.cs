namespace BlockchainNet.Messenger
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using BlockchainNet.Core;
    using BlockchainNet.Core.Models;
    using BlockchainNet.Core.Interfaces;
    using BlockchainNet.Messenger.Models;
    using BlockchainNet.LiteDB;

    public class MessengerChannels
    {
        private readonly string databaseFile;
        private readonly ICommunicator<MessageInstruction> communicator;
        private readonly IConsensusMethod<MessageInstruction> consensusMethod;
        private readonly ISignatureService signatureService;

        private readonly Dictionary<string, MessengerBlockchain> blockchains;

        public MessengerChannels(
            string databaseFile,
            ICommunicator<MessageInstruction> communicator,
            IConsensusMethod<MessageInstruction> consensusMethod,
            ISignatureService signatureService)
        {
            this.databaseFile = databaseFile;
            this.communicator = communicator;
            this.consensusMethod = consensusMethod;
            this.signatureService = signatureService;

            blockchains = new Dictionary<string, MessengerBlockchain>();
        }

        public IEnumerable<string> GetChannelsList()
        {
            return blockchains.Keys;
        }

        public MessengerBlockchain GetOrCreateBlockchain(string channel)
        {
            return blockchains.GetValueOrDefault(
                channel,
                new MessengerBlockchain(
                    communicator,
                    new LiteDBBlockRepository<MessageInstruction>(databaseFile, channel),
                    consensusMethod,
                    signatureService));
        }
    }
}
