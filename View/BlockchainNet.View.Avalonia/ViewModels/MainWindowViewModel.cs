namespace BlockchainNet.View.Gui.ViewModels
{
    using BlockchainNet.Messenger;
    using BlockchainNet.View.Gui.Interfaces;
    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class MainWindowViewModel : ReactiveObject, IMainWindowViewModel
    {
        private readonly MessengerServiceLocator locator;
        private readonly ObservableCollection<IChannelViewModel> channels;
        private (byte[] publicKey, byte[] privateKey) keys;

        public MainWindowViewModel(
            MessengerServiceLocator locator)
        {
            this.locator = locator;
            channels = new ObservableCollection<IChannelViewModel>();

            LoginCommand = ReactiveCommand.CreateFromTask<(string login, string? password)>(LoginAsync);
            ConnectToCommand = ReactiveCommand.CreateFromTask<string>(ConnectToAsync);
            AddChannelCommand = ReactiveCommand.Create<string>(AddChannel);
            RemoveChannelCommand = ReactiveCommand.Create<string>(RemoveChannel);
        }

        [Reactive]
        public IChannelViewModel? SelectedChannel { get; set; }

        public IEnumerable<IChannelViewModel> Channels => channels;

        public ICommand LoginCommand { get; }

        public ICommand ConnectToCommand { get; }

        public ICommand AddChannelCommand { get; }

        public ICommand RemoveChannelCommand { get; }

        private Task LoginAsync((string login, string? password) loginInfo)
        {
            locator.Communicator.Login = loginInfo.login;
            keys = locator.SignatureService.GetKeysFromPassword(loginInfo.password ?? string.Empty);

            foreach (var channel in channels)
            {
                channel.MessageInputViewModel.PrivateKey = keys.privateKey;
                channel.MessageInputViewModel.PublikKey = keys.publicKey;
            }

            AddChannel(MessengerChannels.SharedChannelName);

            return locator.Communicator.StartAsync().AsTask();
        }

        private Task ConnectToAsync(string nodeId)
        {
            return locator.Communicator.ConnectToAsync(new[] { nodeId });
        }

        private void AddChannel(string name)
        {
            var blockchain = locator.Channels.GetOrCreateBlockchain(name);

            var channel = new ChannelViewModel(name, blockchain, locator.Communicator);
            channel.MessageInputViewModel.PrivateKey = keys.privateKey;
            channel.MessageInputViewModel.PublikKey = keys.publicKey;

            channels.Add(channel);
        }

        private void RemoveChannel(string name)
        {
            var channel = channels.FirstOrDefault(c => c.Name == name);
            if (channel != null)
            {
                _ = channels.Remove(channel);
            }
        }
    }
}
