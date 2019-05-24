namespace BlockchainNet.View.Gui.Views
{
    using System;
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Controls.Notifications;
    using Avalonia.Markup.Xaml;
    using Avalonia.ReactiveUI;
    using BlockchainNet.View.Gui.Interfaces;
    using BlockchainNet.View.Gui.Views.Dialogs;
    using Splat;

    public class MainWindow : ReactiveWindow<IMainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Locator.CurrentMutable.RegisterConstant<INotificationManager>(
				new WindowNotificationManager(this) {
					Position = NotificationPosition.BottomRight,
					MaxItems = 4,
					Margin = new Thickness (0, 0, 15, 40)
				});
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
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


            var inputDialog = new InputDialog();
            
            var nodeId = await inputDialog.ShowDialog<string>(Application.Current.MainWindow);
            if (nodeId != null)
            {
                ViewModel.UserListViewModel.ConnectToCommand.Execute(nodeId);
            }
        }
    }
}
