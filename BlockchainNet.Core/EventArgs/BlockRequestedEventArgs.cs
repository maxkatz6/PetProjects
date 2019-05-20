namespace BlockchainNet.Core.EventArgs
{
    using BlockchainNet.Shared.EventArgs;

    public class BlockRequestedEventArgs : DeferredEventArgs
    {
        public BlockRequestedEventArgs(string blockId)
        {
            BlockId = blockId;
        }

        public string BlockId { get; }
    }
}
