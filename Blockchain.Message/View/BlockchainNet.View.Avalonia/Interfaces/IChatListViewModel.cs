namespace BlockchainNet.View.Gui.Interfaces
{
    using System.Collections.Generic;

    using BlockchainNet.View.Gui.Models;
    
    public interface IChatListViewModel
    {
        IEnumerable<MessageModel> Messages { get; }
    }
}
