namespace BlockchainNet.IO.TCP
{
    using BlockchainNet.IO;
    using BlockchainNet.IO.Models;

    public class TcpClientFactory<T> : ICommunicationClientFactory<T>
    {
        public ICommunicationClient<T> CreateNew(string serverId, ClientInformation responseClient)
        {
            return new TcpClient<T>(serverId, responseClient);
        }
    }
}
