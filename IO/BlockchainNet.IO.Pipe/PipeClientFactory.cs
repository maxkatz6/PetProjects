namespace BlockchainNet.IO.Pipe
{
    using BlockchainNet.IO;

    public class PipeClientFactory<T> : ICommunicationClientFactory<T>
    {
        public ICommunicationClient<T> CreateNew(string serverId, string responseServerId)
        {
            return new PipeClient<T>(serverId, responseServerId);
        }
    }
}
