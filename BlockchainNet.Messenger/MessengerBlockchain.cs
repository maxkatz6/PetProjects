namespace BlockchainNet.Messenger
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using BlockchainNet.Core;
    using BlockchainNet.Core.Models;
    using BlockchainNet.Core.Interfaces;
    using BlockchainNet.Messenger.Models;

    public class MessengerBlockchain : Blockchain<MessageInstruction>
    {
        public MessengerBlockchain(
            Communicator<MessageInstruction> communicator,
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
        public override bool IsValidChain(IEnumerable<Block<MessageInstruction>> recievedChain)
        {
            if (!base.IsValidChain(recievedChain))
            {
                return false;
            }

            var prevBlock = recievedChain.FirstOrDefault();
            if (prevBlock == null)
            {
                throw new ArgumentException("Blocks chain cannot be empty", nameof(recievedChain));
            }

            foreach (var block in recievedChain.Skip(1))
            {
                if (!consensusMethod.VerifyConsensus(block))
                {
                    return false;
                }

                prevBlock = block;
            }
            return true;
        }

        /// <summary>
        /// Добавляет новую транзакцию к списку транзакций
        /// </summary>
        /// <param name="sender">Адресс отправителя</param>
        /// <param name="recipient">Адресс получателя</param>
        /// <param name="amount">Сумма</param>
        /// <returns>Индекс блока, хранящего добаленную транзакцию</returns>
        public void NewTransaction(string sender, string recipient, MessageInstruction message, (byte[] publicKey, byte[] privateKey) keys)
        {
            var date = DateTime.Now;
            var transaction = new Transaction<MessageInstruction>(sender, recipient, keys.publicKey, message, date);
            signatureService.SignTransaction(transaction, keys.privateKey);

            uncommitedTransactions.Add(transaction);
        }
    }
}
