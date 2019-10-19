namespace BlockchainNet.IO
{
    using BlockchainNet.Shared.EventArgs;

    public class MessageReceivedEventArgs<T> : DeferredEventArgs
    {
        public MessageReceivedEventArgs(string clientId, T message)
        {
            ClientId = clientId;
            Message = message;
        }

        /// <summary>
        /// Client id
        /// </summary>
        public string ClientId { get; }

        /// <summary>
        /// Received message
        /// </summary>
        public T Message { get; }
    }
}
