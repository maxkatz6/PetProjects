namespace BlockchainNet.Messenger
{
    using System;
    using System.Collections.Generic;

    using BlockchainNet.Core;
    using BlockchainNet.Core.Models;
    using BlockchainNet.Core.Interfaces;
    using BlockchainNet.Messenger.Models;
    using System.Threading.Tasks;
    using System.Linq;

    public class MessengerBlockchain : Blockchain<MessageInstruction>
    {
        public MessengerBlockchain(
            ICommunicator<MessageInstruction> communicator,
            IBlockRepository<MessageInstruction> blockRepository,
            IConsensusMethod<MessageInstruction> consensusMethod,
            ISignatureService signatureService)
            : base(communicator, blockRepository, consensusMethod, signatureService)
        {
        }

        /// <summary>
        /// Проверка блокчейна
        /// </summary>
        /// <param name="chain">Блокчейн</param>
        /// <returns>True если прошел проверку, иначе False</returns>
        public override ValueTask<bool> IsValidChainAsync(IEnumerable<Block<MessageInstruction>> recievedChain)
        {
            return base.IsValidChainAsync(recievedChain);
        }

        /// <summary>
        /// Добавляет новую транзакцию к списку транзакций
        /// </summary>
        /// <param name="sender">Адресс отправителя</param>
        /// <param name="recipient">Адресс получателя</param>
        /// <param name="amount">Сумма</param>
        /// <returns>Индекс блока, хранящего добаленную транзакцию</returns>
        public async Task NewTransactionAsync(string sender, string recipient, MessageInstruction message, (byte[] publicKey, byte[] privateKey) keys)
        {
            var lastLocalBySender = await blockRepository
                .GetLastTransactionAsync(sender)
                .ConfigureAwait(false);
            if (lastLocalBySender != null
                && !lastLocalBySender.PublicKey.SequenceEqual(keys.publicKey))
            {
                throw new InvalidOperationException("Invalid password for existed sender");
            }

            var transaction = new Transaction<MessageInstruction>(sender, recipient, keys.publicKey, message, DateTime.UtcNow);
            signatureService.SignTransaction(transaction, keys.privateKey);

            uncommitedTransactions.Add(transaction);
        }
    }
}
