namespace BlockchainNet.Core.EventArgs
{
    using BlockchainNet.Shared.EventArgs;

    public class BlockRequestedEventArgs : DeferredEventArgs
    {
        public BlockRequestedEventArgs(string blockId, string? channel)
        {
            BlockId = blockId;
            Channel = channel;
        }

        public string BlockId { get; }
        
        public string? Channel { get; set; }
    }
}
