namespace BlockchainNet.Core.Models
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using ProtoBuf;

    [ProtoContract]
    public class Transaction<TContent>
    {
        [ProtoMember(1)]
        public string Id { get; set; }

        [ProtoMember(2)]
        public byte[] PublicKey { get; }

        [ProtoMember(3)]
        public byte[] Signature { get; set; }

        [ProtoMember(4)]
        public string? Sender { get; }

        [ProtoMember(5)]
        public string Recipient { get; }

        [ProtoMember(6)]
        public TContent Content { get; }

        [ProtoMember(7)]
        public DateTime Date { get; }

        /// <summary>
        /// Приватный конструктор для сериализации
        /// </summary>
        private Transaction()
        {

        }

        /// <summary>
        /// Конструктор транзацкии
        /// </summary>
        /// <param name="sender">Отправитель</param>
        /// <param name="recipient">Получатель</param>
        /// <param name="content">Содержимое</param>
        /// <param name="date">Дата создания</param>
        public Transaction(string? sender, string recipient, byte[] publicKey, TContent content, DateTime date)
        {
            Sender = sender;
            Recipient = recipient;
            PublicKey = publicKey;
            Content = content;
            Date = date;
        }

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