﻿namespace BlockchainNet.IO
{
    using BlockchainNet.IO.Models;
    using System.Threading.Tasks;

public interface ICommunicationClient<T> : ICommunication
{
    ClientInformation ResponseClient { get; }

    ValueTask<bool> SendMessageAsync(T message);
}
}
