namespace BlockchainNet.View.Gui.Views.Controls
{
    using Avalonia;
    using Avalonia.Markup.Xaml;
    using Avalonia.ReactiveUI;

    using BlockchainNet.Core;
    using BlockchainNet.Messenger.Models;
    using BlockchainNet.View.Gui.Interfaces;
    using BlockchainNet.View.Gui.Views.Dialogs;

    public class UserListControl : ReactiveUserControl<IUserListViewModel>
    {
        public UserListControl()
        {
            InitializeComponent();
        }

        private async void ConnectTo_Clicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = new InputDialog();
            
            var nodeId = await dialog.ShowDialog<string>(Application.Current.MainWindow);
            if (nodeId != null)
            {
                ViewModel.ConnectToCommand.Execute(nodeId);
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
