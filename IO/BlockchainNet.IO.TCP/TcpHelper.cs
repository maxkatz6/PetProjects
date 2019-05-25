namespace BlockchainNet.IO.TCP
{
    using System.Linq;
    using System.Net.NetworkInformation;

    public static class TcpHelper
    {
        public const int DefaultPort = 50000;

        public static int GetAvailablePort(int min = DefaultPort, int max = DefaultPort + 1000)
        {
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            var ipEndPoints = ipProperties.GetActiveTcpListeners();
            return Enumerable
                .Range(min, max - min)
                .FirstOrDefault(port => !ipEndPoints
                    .Any(endpoint => endpoint.Port == port));
        }
    }
}
