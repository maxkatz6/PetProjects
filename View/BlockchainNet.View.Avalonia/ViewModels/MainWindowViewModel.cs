namespace BlockchainNet.View.Gui.ViewModels
{
    using BlockchainNet.Messenger;
    using BlockchainNet.View.Gui.Interfaces;
    using ReactiveUI;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class MainWindowViewModel : ReactiveObject, IMainWindowViewModel
    {
        private readonly MessengerServiceLocator locator;

        public MainWindowViewModel(
            MessengerServiceLocator locator)
        {
            this.locator = locator;

            ChatListViewModel = new ChatListViewModel(locator.Blockchain);
            UserListViewModel = new UserListViewModel(locator.Communicator);
            MessageInputViewModel = new MessageInputViewModel(locator.Blockchain, locator.Communicator, UserListViewModel);
            
            LoginCommand = ReactiveCommand.CreateFromTask<(string login, string? password)>(LoginAsync);
        }
        
        public IChatListViewModel ChatListViewModel { get; }

        public IMessageInputViewModel MessageInputViewModel { get; }

        public IUserListViewModel UserListViewModel { get; }

        public ICommand LoginCommand { get; }

        private Task LoginAsync((string login, string? password) loginInfo)
        {
            locator.Communicator.Login = loginInfo.login;
            var (publicKey, privateKey) = locator.SignatureService.GetKeysFromPassword(loginInfo.password ?? string.Empty);
            MessageInputViewModel.PrivateKey = privateKey;
            MessageInputViewModel.PublikKey = publicKey;
            return locator.Communicator.StartAsync().AsTask();
        }
    }
}
