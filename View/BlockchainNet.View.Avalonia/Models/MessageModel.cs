namespace BlockchainNet.View.Gui.Models
{
    public class MessageModel
    {
        public MessageModel(string sender, string message)
        {
            Sender = sender;
            Message = message;
        }

        public string Sender { get; }
        
        public string Message { get; }

        public string? HexColor { get; set; }

        public override string ToString()
        {
            return $"{Sender}: {Message}";
        }
    }
}
