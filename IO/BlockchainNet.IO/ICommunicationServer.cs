namespace BlockchainNet.IO
{
    using System;

public interface ICommunicationServer<T> : ICommunication
{
    bool IsListening { get; }

    event EventHandler<MessageReceivedEventArgs<T>> MessageReceivedEvent;

    event EventHandler<ClientConnectedEventArgs> ClientConnectedEvent;

    event EventHandler<ClientDisconnectedEventArgs> ClientDisconnectedEvent;
}
}
