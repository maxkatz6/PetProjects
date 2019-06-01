namespace BlockchainNet.View.Gui.ViewModels
{
    using System;
    using System.Reactive;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using BlockchainNet.Core;
    using BlockchainNet.IO.Models;
    using BlockchainNet.Messenger;
    using BlockchainNet.Messenger.Models;
    using BlockchainNet.View.Gui.Interfaces;

    using ReactiveUI;

    public class MessageInputViewModel : ReactiveObject, IMessageInputViewModel
    {
        private readonly ReactiveCommand<string, Unit> _sendMessage;
        private readonly MessengerBlockchain _blockchain;
        private readonly Communicator<MessageInstruction> _communicator;

        public MessageInputViewModel(
            MessengerBlockchain blockchain,
            Communicator<MessageInstruction> communicator,
            IUserListViewModel userListViewModel)
        {
            _blockchain = blockchain;
            _communicator = communicator;
            _sendMessage = ReactiveCommand.CreateFromTask<string>(m => AddMessage(m, userListViewModel.SelectedClient?.DisplayName));
        }

        public ICommand SendMessageCommand => _sendMessage;

        public byte[]? PrivateKey { get; set; }

        public byte[]? PublikKey { get; set; }

        private Task AddMessage(string message, string? recipient)
        {
            var instruction = new MessageInstruction(message);
            if (_communicator.Login == null)
            {
                throw new InvalidOperationException($"Attempt to send message without {nameof(_communicator.Login)}");
            }
            if (recipient == null)
            {
                throw new ArgumentNullException(nameof(recipient));
            }
            if (PublikKey == null)
            {
                throw new ArgumentNullException(nameof(PublikKey));
            }
            if (PrivateKey == null)
            {
                throw new ArgumentNullException(nameof(PrivateKey));
            }
            _blockchain.NewTransaction(_communicator.Login, recipient, instruction, (PublikKey, PrivateKey));
            return _blockchain.MineAsync(default);
        }
    }
}
