namespace BlockchainNet.View.Gui.ViewModels
{
    using Avalonia.Threading;
    using BlockchainNet.Core;
    using BlockchainNet.IO.Models;
    using BlockchainNet.Messenger.Models;
    using BlockchainNet.View.Gui.Interfaces;

    using ReactiveUI;
    using ReactiveUI.Fody.Helpers;

    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class UserListViewModel : IUserListViewModel
    {
        private readonly ObservableCollection<ClientInformation> clients;
        private readonly Communicator<MessageInstruction> communicator;

        public UserListViewModel(Communicator<MessageInstruction> communicator)
        {
            this.communicator = communicator;
            clients = new ObservableCollection<ClientInformation>();
            Clients = clients;

            communicator.ClientConnected += Communicator_ClientConnected;

            ConnectToCommand = ReactiveCommand.CreateFromTask<string>(ConnectToAsync);

            //var clientInformation = new ClientInformation("127.0.0.1:50001", "max");
            //clients.Add(clientInformation);
            //if (SelectedClient == null)
            //{
            //    SelectedClient = clientInformation;
            //}
        }

        [Reactive]
        public ObservableCollection<ClientInformation> Clients { get; private set; }

        [Reactive]
        public ClientInformation? SelectedClient { get; set; }

        public ICommand ConnectToCommand { get; }

        private void Communicator_ClientConnected(object sender, ClientInformation clientInformation)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                clients.Add(clientInformation);
                Clients = clients;
                if (SelectedClient == null)
                {
                    SelectedClient = clientInformation;
                }
            });
        }

        private Task ConnectToAsync(string nodeId)
        {
            return communicator.ConnectToAsync(new[] { nodeId });
        }
    }
}
