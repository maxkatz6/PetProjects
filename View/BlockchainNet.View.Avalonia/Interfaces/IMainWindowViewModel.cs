namespace BlockchainNet.View.Gui.Interfaces
{
    using System.Collections.Generic;
    using System.Windows.Input;

    public interface IMainWindowViewModel
    {
        IEnumerable<IChannelViewModel> Channels { get; }

        IChannelViewModel? SelectedChannel { get; set; }

        ICommand LoginCommand { get; }

        ICommand AddChannelCommand { get; }

        ICommand RemoveChannelCommand { get; }

        ICommand ConnectToCommand { get; }
    }
}
