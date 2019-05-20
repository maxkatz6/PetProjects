namespace BlockchainNet.Core.Models
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;

    using BlockchainNet.Core.Enum;

    public class Block<TInstruction>
    {
        /// <summary>
        /// конструктор для сериализации
        /// </summary>
        public Block()
        {
            Transactions = new List<Transaction<TInstruction>>();
        }

        /// <summary>
        /// Конструктор блока
        /// </summary>
        /// <param name="id">Индекс блока</param>
        /// <param name="date">Дата создания</param>
        /// <param name="transactions">Транзакции</param>
        /// <param name="proof">Доказательство доберия блоку</param>
        /// <param name="previousBlockId">Хэш предыдущего блока</param>
        public Block(IEnumerable<Transaction<TInstruction>> transactions, string? previousBlockId, uint height)
        {
            Transactions = transactions?.ToList() ?? new List<Transaction<TInstruction>>();
            PreviousBlockId = previousBlockId;
            Height = height;
        }

        public string? Id { get; set; }

        public uint Height { get; set; }

        public DateTimeOffset Date { get; set; }

        public IReadOnlyList<Transaction<TInstruction>> Transactions { get; set; }

        public BlockStatus Status { get; set; }

        public long Proof { get; set; }

        public string? PreviousBlockId { get; set; }

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
