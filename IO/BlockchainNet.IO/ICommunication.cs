namespace BlockchainNet.IO
{
    using System;
    using System.Threading.Tasks;

    public interface ICommunication : IAsyncDisposable
    {
        string ServerId { get; }

        ValueTask StartAsync();

        ValueTask StopAsync();

        ValueTask IAsyncDisposable.DisposeAsync()
        {
            return StopAsync();
        }
    }
}
