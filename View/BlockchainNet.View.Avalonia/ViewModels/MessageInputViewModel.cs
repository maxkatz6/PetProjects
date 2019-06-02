namespace BlockchainNet.View.Gui.ViewModels
{
    using System;
    using System.Reactive;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using BlockchainNet.Core.Interfaces;
    using BlockchainNet.Core.Models;
    using BlockchainNet.Messenger;
    using BlockchainNet.Messenger.Models;
    using BlockchainNet.View.Gui.Interfaces;

    using ReactiveUI;

    public class MessageInputViewModel : ReactiveObject, IMessageInputViewModel
    {
        private readonly ReactiveCommand<string, Unit> _sendMessage;
        private readonly MessengerBlockchain _blockchain;
        private readonly ICommunicator<MessageInstruction> _communicator;

        public MessageInputViewModel(
            MessengerBlockchain blockchain,
            ICommunicator<MessageInstruction> communicator)
        {
            _blockchain = blockchain;
            _communicator = communicator;
            _sendMessage = ReactiveCommand.CreateFromTask<string>(AddMessage);
        }

        public ICommand SendMessageCommand => _sendMessage;

        public byte[]? PrivateKey { get; set; }

        public byte[]? PublikKey { get; set; }

        private async Task AddMessage(string message)
        {
            var instruction = new MessageInstruction(message);
            if (_communicator.Login == null)
            {
                throw new InvalidOperationException($"Attempt to send message without {nameof(_communicator.Login)}");
            }
            if (PublikKey == null)
            {
                throw new ArgumentNullException(nameof(PublikKey));
            }
            if (PrivateKey == null)
            {
                throw new ArgumentNullException(nameof(PrivateKey));
            }
            await _blockchain.NewTransactionAsync(_communicator.Login, Transaction<MessageInstruction>.BroadcastRecipient, instruction, (PublikKey, PrivateKey));
            _ = await _blockchain.MineAsync(default);
        }
    }
}
