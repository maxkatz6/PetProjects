namespace BlockchainNet.View.Gui.ViewModels
{
    using Avalonia;
    using Avalonia.Threading;
    using BlockchainNet.Messenger;
    using BlockchainNet.Messenger.Models;
    using BlockchainNet.View.Gui.Interfaces;
    using BlockchainNet.View.Gui.Models;
    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class ChatListViewModel : ReactiveObject, IChatListViewModel
    {
        private readonly ObservableCollection<MessageModel> _messanges;

        public ChatListViewModel(MessengerBlockchain blockchain)
        {
            _messanges = new ObservableCollection<MessageModel>();
            _messanges.Add(new MessageModel("System", "Welcome!"));
            Messages = _messanges;
            blockchain.BlockAdded += Blockchain_BlockAdded;
        }
        
        [Reactive]
        public ObservableCollection<MessageModel> Messages { get; private set; }

        private void Blockchain_BlockAdded(object sender, Core.EventArgs.BlockAddedEventArgs<MessageInstruction> e)
        {
            var transactions = e.AddedBlocks.SelectMany(b => b.Transactions);
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                foreach (var transaction in transactions)
                {
                    _messanges.Add(new MessageModel(transaction.Sender, transaction.Content.Message)
                    {
                        HexColor = transaction.Content.HexColor
                    });
                    Messages = _messanges;
                }
            });
        }
    }
}
