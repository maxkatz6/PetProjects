namespace BlockchainNet.View.Gui.Views.Controls
{
    using Avalonia.Markup.Xaml;
    using Avalonia.ReactiveUI;
    using BlockchainNet.View.Gui.Interfaces;

    public class ChatListControl : ReactiveUserControl<IChatListViewModel>
    {
        public ChatListControl()
        {
            InitializeComponent();
            Tapped += ChatListControl_Tapped;
        }

        private void ChatListControl_Tapped(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
