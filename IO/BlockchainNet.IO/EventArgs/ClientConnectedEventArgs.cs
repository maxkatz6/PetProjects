namespace BlockchainNet.IO
{
    using BlockchainNet.Shared.EventArgs;

    public class ClientConnectedEventArgs : DeferredEventArgs
    {
        public ClientConnectedEventArgs(string? clientId)
        {
            ClientId = clientId;
        }

        /// <summary>
        /// Client id
        /// </summary>
        public string? ClientId { get; }
    }
}
