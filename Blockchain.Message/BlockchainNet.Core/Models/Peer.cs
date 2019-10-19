namespace BlockchainNet.Core.Models
{
    using System;

    public class Peer
    {
        public string? IpEndpoint { get; set; }

        public DateTime? LastMessaged { get; set; }
    }
}
