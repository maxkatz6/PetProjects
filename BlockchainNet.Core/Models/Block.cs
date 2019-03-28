namespace BlockchainNet.Core.Models
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;

    using ProtoBuf;

    using BlockchainNet.Core.Enum;

    [ProtoContract]
    public class Block<TContent>
    {
        [ProtoMember(1)]
        public string Id { get; private set; }

        [ProtoMember(2)]
        public DateTime Date { get; private set; }

        [ProtoMember(3)]
        private readonly List<Transaction<TContent>> content;

        [ProtoIgnore]
        public IReadOnlyList<Transaction<TContent>> Content => content ?? new List<Transaction<TContent>>();

        public BlockStatus Status { get; private set; }

        [ProtoMember(4)]
        public long Proof { get; private set; }

        [ProtoMember(5)]
        public string? PreviousBlockId { get; private set; }

        /// <summary>
        /// Приватные конструктор для сериализации
        /// </summary>
        private Block()
        {

        }

        /// <summary>
        /// Конструктор блока
        /// </summary>
        /// <param name="id">Индекс блока</param>
        /// <param name="date">Дата создания</param>
        /// <param name="transactions">Транзакции</param>
        /// <param name="proof">Доказательство доберия блоку</param>
        /// <param name="previousBlockId">Хэш предыдущего блока</param>
        public Block(IEnumerable<Transaction<TContent>> transactions, string? previousBlockId)
        {
            content = transactions?.ToList() ?? new List<Transaction<TContent>>();
            PreviousBlockId = previousBlockId;
        }

        public void ConfirmBlock(string blockId, long proof)
        {
            Date = DateTime.UtcNow;
            Id = blockId;
            Proof = proof;
            Status = BlockStatus.Confirmed;
        }

        public byte[] GetHash(long nonce)
        {
            var hashString = $"{PreviousBlockId}{nonce}";
            return Encoding.UTF8.GetBytes(hashString);
        }

        public override string ToString()
        {
            var str = $"Block #{Id} {Date}\r\n";
            return str + $"Proof = {Proof}; Previous hash = {PreviousBlockId}";
        }
    }
}
