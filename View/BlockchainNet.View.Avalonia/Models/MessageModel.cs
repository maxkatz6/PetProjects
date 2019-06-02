namespace BlockchainNet.View.Gui.Models
{
    using System;

    public class MessageModel
    {
        public MessageModel(string sender, string message, DateTime dateTime)
        {
            Sender = sender;
            Message = message;
            DateTime = dateTime;
        }

        public DateTime DateTime { get; }

        public string Sender { get; }

        public string Message { get; }

        public string? HexColor { get; set; }

        public override string ToString()
        {
            return $"[{DateTime:yyyy-MM-dd}] ({Sender}): {Message}";
        }
    }
}
