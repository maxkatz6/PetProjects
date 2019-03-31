namespace BlockchainNet.Core
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Security.Cryptography;

    using BlockchainNet.Core.Enum;
    using BlockchainNet.Core.Models;
    using BlockchainNet.Core.Interfaces;

    public abstract class Blockchain<TContent>
    {
        protected readonly IConsensusMethod<TContent> consensusMethod;
        protected readonly ISignatureService signatureService;

        protected Blockchain(IConsensusMethod<TContent> consensusMethod, ISignatureService signatureService)
        {
            this.consensusMethod = consensusMethod;
            this.signatureService = signatureService;

            chain = new List<Block<TContent>>();
            currentTransactions = new List<Transaction<TContent>>();
        }

        protected List<Block<TContent>> chain;
        protected List<Transaction<TContent>> currentTransactions;

        /// <summary>
        /// Цепочка блоков, блокчейн
        /// </summary>
        public IReadOnlyList<Block<TContent>> Chain => chain ?? new List<Block<TContent>>();

        /// <summary>
        /// Текущие транзакции
        /// </summary>
        public IReadOnlyList<Transaction<TContent>> CurrentTransactions => currentTransactions ?? new List<Transaction<TContent>>();

        public event EventHandler<BlockAddedEventArgs<TContent>> BlockAdded;

        // public event EventHandler BlockchainReplaced;

        /// <returns>Последний блок в цепочке</returns>
        public Block<TContent> LastBlock()
        {
            return chain.Last();
        }

        /// <summary>
        /// Запускает процесс майнинга нового блока
        /// </summary>
        /// <returns>Новый блок</returns>
        public async Task<Block<TContent>> MineAsync(string minerAccount, CancellationToken cancellationToken)
        {
            var lastBlock = LastBlock();
            var lastProof = lastBlock.Proof;

            var block = new Block<TContent>(
                currentTransactions,
                lastBlock.Id);

            await consensusMethod.BuildConsensus(block, cancellationToken).ConfigureAwait(false);

            if (block.Status == BlockStatus.Confirmed)
            {
                chain.Add(block);

                currentTransactions.Clear();

                OnMined(minerAccount, block);
                BlockAdded?.Invoke(this, new BlockAddedEventArgs<TContent>(block, chain));
            }

            return block;
        }

        /// <summary>
        /// Пытается заменить блокчейн
        /// </summary>
        /// <param name="recievedChain">Новый блокчейн</param>
        /// <returns>True, если новый блок валидный и замена успешна, иначе - False</returns>
        public bool TrySetChainIfValid(IReadOnlyCollection<Block<TContent>> recievedChain)
        {
            if (IsValidChain(recievedChain))
            {
                var oldChain = chain;
                chain = recievedChain.ToList();
                var newBlocks = recievedChain.SkipWhile(block => oldChain.Any(b => b.Id == block.Id));
                foreach (var newBlock in newBlocks)
                {
                    BlockAdded?.Invoke(this, new BlockAddedEventArgs<TContent>(newBlock, newBlocks));
                }
                // BlockchainReplaced?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }

        public virtual bool IsValidChain(IReadOnlyCollection<Block<TContent>> recievedChain)
        {
            if (recievedChain.Count < chain.Count)
            {
                return false;
            }

            return recievedChain.SelectMany(block => block.Content)
                .All(transaction => signatureService.VerifyTransaction(transaction));
        }

        protected virtual void OnMined(string minerAccount, Block<TContent> block)
        {

        }

        protected void GenerateGenesis()
        {
            if (chain.Count > 0)
            {
                throw new InvalidOperationException();
            }
            var block = new Block<TContent>(currentTransactions, null);
            using (var h = SHA256.Create())
            {
                var hash = h.ComputeHash(block.GetHash(0));
                block.ConfirmBlock(Convert.ToBase64String(hash), 0);
                chain.Add(block);

                currentTransactions.Clear();
                BlockAdded?.Invoke(this, new BlockAddedEventArgs<TContent>(block, chain));
            }
        }
    }
}