namespace BlockchainNet.IO.TCP
{
    using BlockchainNet.IO;

    public class TcpClientFactory<T> : ICommunicationClientFactory<T>
    {
        public ICommunicationClient<T> CreateNew(string serverId, string responseServerId)
        {
            return new TcpClient<T>(serverId, responseServerId);
        }
    }
}
