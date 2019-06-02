namespace BlockchainNet.Core
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using BlockchainNet.Core.Enum;
    using BlockchainNet.Core.Models;
    using BlockchainNet.Core.Interfaces;
    using BlockchainNet.Shared.EventArgs;
    using BlockchainNet.Core.EventArgs;

    public abstract class Blockchain<TInstruction>
    {
        public const string RootId = "0";

        protected readonly IConsensusMethod<TInstruction> consensusMethod;
        protected readonly ISignatureService signatureService;
        protected readonly IBlockRepository<TInstruction> blockRepository;
        protected readonly ICommunicator<TInstruction> communicator;

        protected readonly List<Transaction<TInstruction>> uncommitedTransactions;

        protected Blockchain(
            ICommunicator<TInstruction> communicator,
            IBlockRepository<TInstruction> blockRepository,
            IConsensusMethod<TInstruction> consensusMethod,
            ISignatureService signatureService)
        {
            this.communicator = communicator;
            this.blockRepository = blockRepository;
            this.consensusMethod = consensusMethod;
            this.signatureService = signatureService;

            BlockchainChannel = blockRepository.Channel;

            uncommitedTransactions = new List<Transaction<TInstruction>>();

            communicator.BlockReceived += Communicator_BlockAdded;
            communicator.BlockRequested += Communicator_BlockRequested;
        }

        /// <summary>
        /// Текущие транзакции
        /// </summary>
        public IReadOnlyList<Transaction<TInstruction>> CurrentTransactions => uncommitedTransactions ?? new List<Transaction<TInstruction>>();

        public event EventHandler<BlockAddedEventArgs<TInstruction>> BlockAdded;

        public string? BlockchainChannel { get; }

        /// <summary>
        /// Запускает процесс майнинга нового блока
        /// </summary>
        /// <returns>Новый блок</returns>
        public async Task<Block<TInstruction>> MineAsync(CancellationToken cancellationToken)
        {
            var topBlock = await blockRepository.GetTopBlock().ConfigureAwait(false)
                ?? await GenerateGenesisAsync().ConfigureAwait(false);
            if (string.IsNullOrEmpty(topBlock.Id)
                || topBlock.Status != BlockStatus.Confirmed)
            {
                throw new InvalidOperationException($"Previous block is not confirmed. Id = {topBlock.Id}");
            }

            var lastProof = topBlock.Proof;

            var block = new Block<TInstruction>(uncommitedTransactions, topBlock.Id, topBlock.Height + 1);

            await consensusMethod.BuildConsensus(block, cancellationToken).ConfigureAwait(false);

            if (block.Status == BlockStatus.Confirmed)
            {
                await blockRepository.AddBlock(block).ConfigureAwait(false);

                uncommitedTransactions.Clear();

                BlockAdded?.Invoke(this, new BlockAddedEventArgs<TInstruction>(new[] { block }, BlockchainChannel));
                _ = await communicator.BroadcastBlocksAsync(new[] { block }, BlockchainChannel).ConfigureAwait(false);
            }

            return block;
        }

        public IAsyncEnumerable<Block<TInstruction>> GetFork(string? blockId)
        {
            return blockRepository.GetFork(blockId ?? RootId);
        }

        public async Task<bool> HasBlock(string blockId)
        {
            return await blockRepository.GetBlock(blockId) != null;
        }

        /// <summary>
        /// Пытается заменить блокчейн
        /// </summary>
        /// <param name="recievedChain">Новый блокчейн</param>
        /// <returns>True, если новый блок валидный и замена успешна, иначе - False</returns>
        public async Task<bool> TrySetChainIfValid(Block<TInstruction> block)
        {
            if (block.Id == RootId
                || string.IsNullOrEmpty(block.PreviousBlockId))
            {
                throw new InvalidOperationException("Attempt to set genesis block");
            }

            if (!await IsValidChainAsync(new[] { block }))
            {
                return false;
            }

            var prevBlock = await blockRepository.GetBlock(block.PreviousBlockId).ConfigureAwait(false);
            var lastBlock = await blockRepository.GetTopBlock().ConfigureAwait(false);
            var isEmpty = await blockRepository.IsEmpty().ConfigureAwait(false);

            if (prevBlock != null)
            {
                if (string.IsNullOrEmpty(prevBlock.Id)
                    || prevBlock.Status != BlockStatus.Confirmed)
                {
                    throw new InvalidOperationException($"Previous block is not confirmed. Id = {prevBlock.Id}");
                }

                if (block.Date < prevBlock.Date)
                {
                    return false;
                }
                if (block.Height != (prevBlock.Height + 1))
                {
                    return false;
                }

                if (prevBlock.Id == lastBlock?.Id)
                {
                    await blockRepository.AddBlock(block).ConfigureAwait(false);
                    return true;
                }
                else
                {
                    var forkedTransactions = await blockRepository.GetFork(prevBlock.Id)
                        .Skip(1)
                        .SelectMany(b => b.Transactions.ToAsyncEnumerable())
                        .ToArrayAsync()
                        .ConfigureAwait(false);
                    await blockRepository.RewindChain(prevBlock.Id).ConfigureAwait(false);
                    await blockRepository.AddBlock(block).ConfigureAwait(false);
                    uncommitedTransactions.AddRange(forkedTransactions);
                    return true;
                }
            }
            else if (isEmpty)
            {
                if (block.PreviousBlockId != null && block.PreviousBlockId != RootId)
                {
                    _ = await communicator.BroadcastRequestAsync(RootId, BlockchainChannel).ConfigureAwait(false);
                    return false;
                }
                else
                {
                    _ = await GenerateGenesisAsync().ConfigureAwait(false);
                    await blockRepository.AddBlock(block).ConfigureAwait(false);
                    return true;
                }
            }
            else
            {
                _ = await communicator.BroadcastRequestAsync(block.PreviousBlockId, BlockchainChannel).ConfigureAwait(false);
                return false;
            }
        }

        public virtual ValueTask<bool> IsValidChainAsync(IEnumerable<Block<TInstruction>> recievedChain)
        {
            var consensusValid = recievedChain.Any(block => !consensusMethod.VerifyConsensus(block));
            if (consensusValid)
            {
                return new ValueTask<bool>(false);
            }

            return recievedChain
                .SelectMany(block => block.Transactions)
                .ToAsyncEnumerable()
                .AllAwaitAsync(async transaction =>
                {
                    if (!signatureService.VerifyTransaction(transaction))
                    {
                        return false;
                    }
                    var lastLocalBySender = await blockRepository.GetLastTransactionAsync(transaction.Sender).ConfigureAwait(false);
                    if (lastLocalBySender == null)
                    {
                        return true;
                    }
                    return lastLocalBySender.PublicKey.SequenceEqual(transaction.PublicKey);
                });
        }

        private async Task<Block<TInstruction>> GenerateGenesisAsync()
        {
            var isEmpty = await blockRepository.IsEmpty().ConfigureAwait(false);
            if (!isEmpty)
            {
                throw new InvalidOperationException("Genesis block already exist");
            }

            var block = new Block<TInstruction>(new Transaction<TInstruction>[0], null, 0);

            block.ConfirmBlock(RootId, 0);
            await blockRepository.AddBlock(block).ConfigureAwait(false);

            await BlockAdded
                .InvokeAsync(this, new BlockAddedEventArgs<TInstruction>(new[] { block }, BlockchainChannel))
                .ConfigureAwait(false);

            return block;
        }

        private async void Communicator_BlockRequested(object sender, BlockRequestedEventArgs e)
        {
            if (e.Channel != BlockchainChannel)
            {
                return;
            }
            using var _ = e.GetDeferral();

            var fork = await blockRepository.GetFork(e.BlockId).ToListAsync().ConfigureAwait(false);
            var count = await communicator.BroadcastBlocksAsync(fork, e.Channel, peer => peer.IpEndpoint == e.ClientId).ConfigureAwait(false);
        }

        private async void Communicator_BlockAdded(object sender, BlockReceivedEventArgs<TInstruction> e)
        {
            if (e.Channel != BlockchainChannel)
            {
                throw new InvalidOperationException("Attemt to receive block from invalid channel");
            }

            using (e.GetDeferral())
            {
                var lastBlock = await blockRepository.GetTopBlock().ConfigureAwait(false);
                try
                {
                    foreach (var block in e.AddedBlocks.SkipWhile(b => b.Id == RootId))
                    {
                        var result = await TrySetChainIfValid(block).ConfigureAwait(false);
                        if (!result)
                        {
                            return;
                        }
                    }
                }
                catch
                {
                    await blockRepository.RewindChain(lastBlock?.Id ?? RootId).ConfigureAwait(false);
                    throw;
                }
                _ = await communicator.BroadcastBlocksAsync(e.AddedBlocks, BlockchainChannel, peer => peer.IpEndpoint != e.ClientId).ConfigureAwait(false);
                await BlockAdded.InvokeAsync(this, new BlockAddedEventArgs<TInstruction>(e.AddedBlocks, e.Channel)).ConfigureAwait(false);
            }
        }
    }
}
