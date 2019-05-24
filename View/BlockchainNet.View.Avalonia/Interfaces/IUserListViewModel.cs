namespace BlockchainNet.View.Gui.Interfaces
{
    using BlockchainNet.IO.Models;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public interface IUserListViewModel
    {
        ObservableCollection<ClientInformation> Clients { get; }

        ClientInformation? SelectedClient { get; set; }

        ICommand ConnectToCommand { get; }
    }
}
