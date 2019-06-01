namespace BlockchainNet.IO
{
    using BlockchainNet.IO.Models;
    using System.Threading.Tasks;

    public interface ICommunicationClient<T> : ICommunication
    {
        ClientInformation ResponseClient { get; }

        /// <summary>
        /// Асинхронный метод для отправки сообщения
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns>True если отправка успещна, иначе False</returns>
        ValueTask<bool> SendMessageAsync(T message);
    }
}
