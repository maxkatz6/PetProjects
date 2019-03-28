namespace BlockchainNet.Messenger
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using BlockchainNet.Core;
    using BlockchainNet.Core.Models;
    using BlockchainNet.Core.Interfaces;

    public class MessengerBlockchain : Blockchain<string>
    {
        public MessengerBlockchain(IConsensusMethod<string> consensusMethod)
            : base(consensusMethod)
        {
            GenerateGenesis();
        }

        /// <summary>
        /// Проверка блокчейна
        /// </summary>
        /// <param name="chain">Блокчейн</param>
        /// <returns>True если прошел проверку, иначе False</returns>
        public override bool IsValidChain(IReadOnlyCollection<Block<string>> recievedChain)
        {
            var prevBlock = chain.FirstOrDefault();
            if (prevBlock == null)
            {
                throw new ArgumentException("Blocks chain cannot be empty", nameof(chain));
            }

            foreach (var block in chain.Skip(1))
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
        public void NewTransaction(string sender, string recipient, string message)
        {
            var date = DateTime.Now;
            var transaction = new Transaction<string>(sender, recipient, message, date);
            currentTransactions.Add(transaction);
        }

        protected override void OnMined(string minerAccount, Block<string> block)
        {
            // Оплата за майнинг
            //var transaction = new Transaction<decimal>(null, minerAccount, 1m, DateTime.Now);
            //currentTransactions.Add(transaction);
        }
    }
}
