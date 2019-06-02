namespace BlockchainNet.View.Gui.Views
{
    using System;
    using Avalonia;
    using Avalonia.Interactivity;
    using Avalonia.Markup.Xaml;
    using BlockchainNet.View.Gui.Interfaces;
    using BlockchainNet.View.Gui.Views.Dialogs;

    public class MainWindow : ReactiveWindow<IMainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void AddChannel_Click(object sender, RoutedEventArgs e)
        {
            var inputDialog = new InputDialog();

            var nodeId = await inputDialog.ShowDialog<string>(Application.Current.MainWindow);
            if (nodeId != null)
            {
                ViewModel.AddChannelCommand.Execute(nodeId);
            }
        }

        private async void AddPeer_Click(object sender, RoutedEventArgs e)
        {
            var inputDialog = new InputDialog();

            var nodeId = await inputDialog.ShowDialog<string>(Application.Current.MainWindow);
            if (nodeId != null)
            {
                ViewModel.ConnectToCommand.Execute(nodeId);
            }
        }

        private async void MainWindow_Initialized(object sender, EventArgs e)
        {
            var dialog = new LoginDialog();
            var (login, password) = await dialog.ShowDialog<(string? login, string? password)>(this);
            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentNullException(nameof(login));
            }
            ViewModel.LoginCommand.Execute((login, password));
        }
    }
}
