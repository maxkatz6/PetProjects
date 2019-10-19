namespace BlockchainNet.IO.Models
{
    public class ClientInformation
    {
        public ClientInformation(string clientId, string? displayName = null)
        {
            ClientId = clientId;
            DisplayName = displayName;
        }

        public string ClientId { get; }

        public string? DisplayName { get; }

        public override string ToString()
        {
            return DisplayName ?? "Unknown";
        }
    }
}
