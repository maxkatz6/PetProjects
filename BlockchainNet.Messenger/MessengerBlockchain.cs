namespace BlockchainNet.Messenger
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;

    using BlockchainNet.Core;
    using BlockchainNet.Core.Models;

    using ProtoBuf;

    public class MessengerBlockchain : Blockchain<string>
    {
        /// <summary>
        /// Создает блокчейн с одним генезис блоком
        /// </summary>
        /// <returns>Созданный блокчейн</returns>
        public static MessengerBlockchain CreateNew()
        {
            var blockchain = new MessengerBlockchain();
            blockchain.NewBlock(100, "1");
            return blockchain;
        }

        /// <summary>
        /// Читает и создает блокчейн из файла
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <returns>Прочитанный блокчейн</returns>
        public static MessengerBlockchain FromFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty", nameof(fileName));
            }

            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                return Serializer.Deserialize<MessengerBlockchain>(file);
            }
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

            var amountDictionary = new Dictionary<string, decimal>();

            foreach (var block in chain.Skip(1))
            {
                if (block.PreviousHash != Crypto.HashBlockInBase64(prevBlock)
                    || !IsValidProof(prevBlock.Proof, block.Proof))
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
        public int NewTransaction(string sender, string recipient, string message)
        {
            var date = DateTime.Now;
            var transaction = new Transaction<string>(sender, recipient, message, date);
            currentTransactions.Add(transaction);
            return LastBlock().Index + 1;
        }

        protected override void OnMined(string minerAccount, long proof)
        {
            // Оплата за майнинг
            //var transaction = new Transaction<decimal>(null, minerAccount, 1m, DateTime.Now);
            //currentTransactions.Add(transaction);
        }
    }
}
