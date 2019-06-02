namespace BlockchainNet.View.Gui.Interfaces
{
    using System.Windows.Input;

    public interface IMessageInputViewModel
    {
        ICommand SendMessageCommand { get; }

        public byte[]? PrivateKey { get; set; }

        public byte[]? PublikKey { get; set; }
    }
}
