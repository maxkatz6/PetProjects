namespace BlockchainNet.Messenger.Models
{
    using System;

    [Serializable]
    public class MessageInstruction
    {
        public MessageInstruction(string message)
        {
            Message = message;
        }

        public string? HexColor { get; set; }

        public string Message { get; }
    }
}
