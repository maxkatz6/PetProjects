namespace BlockchainNet.IO
{
    using BlockchainNet.Shared.EventArgs;

    public class ClientDisconnectedEventArgs : DeferredEventArgs
    {
        public ClientDisconnectedEventArgs(string? clientId)
        {
            ClientId = clientId;
        }

        /// <summary>
        /// Client id
        /// </summary>
        public string? ClientId { get; }
    }
}
