namespace BlockchainNet.IO
{
    using System;
    using System.Threading.Tasks;

    public interface ICommunication : IAsyncDisposable
    {
        /// <summary>
        /// Id сервера
        /// </summary>
        string ServerId { get; }

        /// <summary>
        /// Запускает коммуникацию
        /// </summary>
        ValueTask StartAsync();

        /// <summary>
        /// Остагавливает коммуникацию
        /// </summary>
        ValueTask StopAsync();

        ValueTask IAsyncDisposable.DisposeAsync()
        {
            return StopAsync();
        }
    }
}
