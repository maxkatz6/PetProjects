﻿namespace BlockchainNet.Wallet
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;

    using BlockchainNet.Core;
    using BlockchainNet.Core.Models;

    using ProtoBuf;

    public class WalletBlockchain : Blockchain<decimal>
    {
        /// <summary>
        /// Создает блокчейн с одним генезис блоком
        /// </summary>
        /// <returns>Созданный блокчейн</returns>
        public static WalletBlockchain CreateNew()
        {
            var blockchain = new WalletBlockchain();
            blockchain.NewBlock(100, "1");
            return blockchain;
        }

        /// <summary>
        /// Читает и создает блокчейн из файла
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <returns>Прочитанный блокчейн</returns>
        public static WalletBlockchain FromFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty", nameof(fileName));
            }

            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                return Serializer.Deserialize<WalletBlockchain>(file);
            }
        }

        /// <summary>
        /// Проверка блокчейна
        /// </summary>
        /// <param name="chain">Блокчейн</param>
        /// <returns>True если прошел проверку, иначе False</returns>
        public override bool IsValidChain(IReadOnlyCollection<Block<decimal>> recievedChain)
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

                foreach (var content in block.Content)
                {
                    if (!string.IsNullOrEmpty(content.Sender))
                    {
                        if (amountDictionary.ContainsKey(content.Sender))
                        {
                            amountDictionary[content.Sender] -= content.Content;
                            if (amountDictionary[content.Sender] < 0)
                            {
                                return false;
                            }
                        }
                        else if (content.Content > 0)
                        {
                            return false;
                        }
                    }
                    if (amountDictionary.ContainsKey(content.Recipient))
                    {
                        amountDictionary[content.Recipient] += content.Content;
                    }
                    else
                    {
                        amountDictionary.Add(content.Recipient, content.Content);
                    }
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
        public int NewTransaction(string sender, string recipient, decimal amount)
        {
            if (GetAccountAmount(sender) < amount)
            {
                throw new InvalidOperationException("Sender amount is not enough");
            }

            var date = DateTime.Now;
            var transaction = new Transaction<decimal>(sender, recipient, amount, date);
            currentTransactions.Add(transaction);
            return LastBlock().Index + 1;
        }

        /// <summary>
        /// Возвращает сумму на счету аккаунта
        /// </summary>
        /// <param name="account">Аккаунт</param>
        /// <returns>Сумма на счету</returns>
        public decimal GetAccountAmount(string account)
        {
            if (string.IsNullOrEmpty(account))
            {
                throw new ArgumentException("Account cannot be null or empty", nameof(account));
            }

            var amount = 0m;
            foreach (var block in chain)
            {
                foreach (var transaction in block.Content)
                {
                    if (transaction.Recipient == account)
                    {
                        amount += transaction.Content;
                    }

                    if (transaction.Sender == account)
                    {
                        amount -= transaction.Content;
                    }
                }
            }
            if (amount < 0)
            {
                throw new InvalidDataException("Chain is invalid");
            }

            return amount;
        }

        protected override void OnMined(string minerAccount, long proof)
        {
            // Оплата за майнинг
            var transaction = new Transaction<decimal>(null, minerAccount, 1m, DateTime.Now);
            currentTransactions.Add(transaction);
        }
    }
}
