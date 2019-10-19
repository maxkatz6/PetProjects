namespace BlockchainNet.View.Gui.Interfaces
{
    public interface IChannelViewModel
    {
        string Name { get; }

        IChatListViewModel ChatListViewModel { get; }

        IMessageInputViewModel MessageInputViewModel { get; }
    }
}
