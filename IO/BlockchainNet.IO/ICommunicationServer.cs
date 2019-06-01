namespace BlockchainNet.IO
{
    using System;

    public interface ICommunicationServer<T> : ICommunication
    {
        bool IsListening { get; }

        /// <summary>
        /// Событие, которое вызывается при получении сообщения
        /// </summary>
        event EventHandler<MessageReceivedEventArgs<T>> MessageReceivedEvent;

        /// <summary>
        /// Событие, которое вызывается при подключении клиента 
        /// </summary>
        event EventHandler<ClientConnectedEventArgs> ClientConnectedEvent;

        /// <summary>
        /// Событие, которое вызывается при отключении клиента 
        /// </summary>
        event EventHandler<ClientDisconnectedEventArgs> ClientDisconnectedEvent;
    }
}
