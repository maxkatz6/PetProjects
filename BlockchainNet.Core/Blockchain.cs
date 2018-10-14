namespace BlockchainNet.Core
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;

    using ProtoBuf;

    using BlockchainNet.Core.Models;

    [ProtoContract]
    public abstract class Blockchain<TContent>
    {
        [ProtoMember(1)]
        protected List<Block<TContent>> chain;

        [ProtoMember(2)]
        protected List<TContent> currentContent;

        /// <summary>
        /// Цепочка блоков, блокчейн
        /// </summary>
        public IReadOnlyList<Block<TContent>> Chain => chain ?? new List<Block<TContent>>();

        /// <summary>
        /// Текущие транзакции
        /// </summary>
        public IReadOnlyList<TContent> CurrentContent => currentContent ?? new List<TContent>();

        public event EventHandler<BlockAddedEventArgs<TContent>> BlockAdded;

        public event EventHandler BlockchainReplaced;

        protected Blockchain()
        {
            chain = new List<Block<TContent>>();
            currentContent = new List<TContent>();
        }

        /// <summary>
        /// Сохраняет текущий блокчейн в файл
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        public void SaveFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty", nameof(fileName));
            }

            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                Serializer.Serialize(file, this);
            }
        }

        /// <returns>Последний блок в цепочке</returns>
        public Block<TContent> LastBlock()
        {
            return chain.Last();
        }

        /// <summary>
        /// Запускает процесс майнинга нового блока
        /// </summary>
        /// <returns>Новый блок</returns>
        public Block<TContent> Mine(string minerAccount)
        {
            var lastBlock = LastBlock();
            var lastProof = lastBlock.Proof;
            var lastHash = Crypto.HashBlockInBase64(lastBlock);

            var proof = ProofOfWork(lastProof);

            OnMined(minerAccount, proof);

            return NewBlock(proof, lastHash);
        }

        protected virtual void OnMined(string minerAccount, long proof)
        {

        }

        /// <summary>
        /// Пытается заменить блокчейн
        /// </summary>
        /// <param name="recievedChain">Новый блокчейн</param>
        /// <returns>True, если новый блок валидный и замена успешна, иначе - False</returns>
        public bool TrySetChainIfValid(IReadOnlyCollection<Block<TContent>> recievedChain)
        {
            if (recievedChain.Count >= chain.Count
                && IsValidChain(recievedChain))
            {
                chain = recievedChain.ToList();
                BlockchainReplaced?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }

        public abstract bool IsValidChain(IReadOnlyCollection<Block<TContent>> recievedChain);

        protected Block<TContent> NewBlock(long proof, string previousHash = null)
        {
            var block = new Block<TContent>(
                chain.Count,
                DateTime.Now,
                currentContent,
                proof,
                previousHash);
            chain.Add(block);

            currentContent.Clear();

            BlockAdded?.Invoke(this, new BlockAddedEventArgs<TContent>(block, chain));

            return block;
        }

        private long ProofOfWork(long lastProof)
        {
            var proof = 0L;

            while (!IsValidProof(lastProof, proof))
            {
                proof++;
            }

            return proof;
        }

        protected static bool IsValidProof(long lastProof, long proof)
        {
            return Crypto.HashString($"{lastProof}{proof}").EndsWith("00");
        }
    }
}