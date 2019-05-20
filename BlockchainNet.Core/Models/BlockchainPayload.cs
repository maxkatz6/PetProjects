namespace BlockchainNet.Core.Models
{
    using System.Collections.Generic;

    using BlockchainNet.Core.Enum;

    public class BlockchainPayload<TInstruction>
    {
        public BlockchainPayloadAction Action { get; set; }

        public string? BlockId { get; set; }

        public IEnumerable<Block<TInstruction>> Blocks { get; set; }
    }
}
