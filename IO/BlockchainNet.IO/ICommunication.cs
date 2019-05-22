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
        Task StartAsync();

        /// <summary>
        /// Остагавливает коммуникацию
        /// </summary>
        Task StopAsync();
    }
}
