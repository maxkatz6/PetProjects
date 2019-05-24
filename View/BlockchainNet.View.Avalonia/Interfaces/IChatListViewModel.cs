namespace BlockchainNet.View.Gui.Interfaces
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using BlockchainNet.View.Gui.Models;
    
    public interface IChatListViewModel
    {
        ObservableCollection<MessageModel> Messages { get; }
    }
}
