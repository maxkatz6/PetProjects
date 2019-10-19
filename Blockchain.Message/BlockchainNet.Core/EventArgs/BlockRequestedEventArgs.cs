namespace BlockchainNet.Core.EventArgs
{
    using BlockchainNet.Shared.EventArgs;

    public class BlockRequestedEventArgs : DeferredEventArgs
    {
        public BlockRequestedEventArgs(string clientId, string blockId, string? channel)
        {
            ClientId = clientId;
            BlockId = blockId;
            Channel = channel;
        }

        public string ClientId { get; }

        public string BlockId { get; }

        public string? Channel { get; set; }
    }
}
