namespace BlockchainNet.View.Gui.ViewModels
{
    using BlockchainNet.Core.Interfaces;
    using BlockchainNet.Messenger;
    using BlockchainNet.Messenger.Models;
    using BlockchainNet.View.Gui.Interfaces;

    public class ChannelViewModel : IChannelViewModel
    {
        public ChannelViewModel(
            string name,
            MessengerBlockchain blockchain,
            ICommunicator<MessageInstruction> communicator)
        {
            Name = name ?? "Shared";
            ChatListViewModel = new ChatListViewModel(blockchain);
            MessageInputViewModel = new MessageInputViewModel(blockchain, communicator);
        }

        public string Name { get; }

        public IChatListViewModel ChatListViewModel { get; }

        public IMessageInputViewModel MessageInputViewModel { get; }
    }
}
