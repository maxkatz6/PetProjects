namespace BlockchainNet.Core.Models
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public class Transaction<TInstruction>
    {
        public const string BroadcastRecipient = "Broadcast";

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        public Transaction()
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
        {

        }

        /// <summary>
        /// Конструктор транзацкии
        /// </summary>
        /// <param name="sender">Отправитель</param>
        /// <param name="recipient">Получатель</param>
        /// <param name="content">Содержимое</param>
        /// <param name="date">Дата создания</param>
        public Transaction(string sender, string recipient, byte[] publicKey, TInstruction content, DateTime date)
        {
            Sender = sender;
            Recipient = recipient;
            PublicKey = publicKey;
            Content = content;
            Date = date;
        }

        public string? Id { get; set; }

        public byte[] PublicKey { get; set; }

        public byte[]? Signature { get; set; }

        public string Sender { get; set; }

        public string Recipient { get; set; }

        public TInstruction Content { get; set; }

        public DateTimeOffset Date { get; set; }

        public byte[] GetHash()
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, (Sender, Recipient, Content));
                return ms.ToArray();
            }
        }

        public override string ToString()
        {
            return $"Transaction from {(string.IsNullOrEmpty(Sender) ? "mining" : Sender)} to {Recipient}:{Environment.NewLine}{Content}";
        }
    }
}
