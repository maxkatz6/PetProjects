namespace BlockchainNet.IO
{
    using BlockchainNet.IO.Models;
    using BlockchainNet.Shared.EventArgs;

    public class ClientConnectedEventArgs : DeferredEventArgs
    {
        public ClientConnectedEventArgs(ClientInformation clientInformation)
        {
            ClientInformation = clientInformation;
        }

        /// <summary>
        /// Client id
        /// </summary>
        public ClientInformation ClientInformation { get; }
    }
}
