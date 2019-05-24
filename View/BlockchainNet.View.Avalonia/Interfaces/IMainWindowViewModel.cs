namespace BlockchainNet.View.Gui.Interfaces
{
    using System.Windows.Input;

    public interface IMainWindowViewModel
    {
        IChatListViewModel ChatListViewModel { get; }

        IMessageInputViewModel MessageInputViewModel { get; }

        IUserListViewModel UserListViewModel { get; }

        ICommand LoginCommand { get; }
    }
}
