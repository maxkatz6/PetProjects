namespace BlockchainNet.IO
{
    using System.Threading.Tasks;

    public interface ICommunication
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
