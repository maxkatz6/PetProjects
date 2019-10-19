namespace BlockchainNet.IO
{
    using BlockchainNet.IO.Models;

    public interface ICommunicationClientFactory<T>
    {
        ICommunicationClient<T> CreateNew(string serverId, ClientInformation responseClient);
    }
}
