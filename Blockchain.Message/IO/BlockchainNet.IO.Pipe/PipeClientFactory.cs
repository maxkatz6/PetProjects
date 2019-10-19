namespace BlockchainNet.IO.Pipe
{
    using BlockchainNet.IO;
    using BlockchainNet.IO.Models;

    public class PipeClientFactory<T> : ICommunicationClientFactory<T>
    {
        public ICommunicationClient<T> CreateNew(string serverId, ClientInformation responseClient)
        {
            return new PipeClient<T>(serverId, responseClient);
        }
    }
}
