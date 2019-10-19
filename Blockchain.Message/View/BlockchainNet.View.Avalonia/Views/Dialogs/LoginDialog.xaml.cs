namespace BlockchainNet.View.Gui.Views.Dialogs
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;

    public class LoginDialog : Window
    {
        public LoginDialog()
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

        private void Ok_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var login = this.FindControl<TextBox>("LoginTextBox")?.Text;
            var password = this.FindControl<TextBox>("PasswordTextBox")?.Text;
            Close((login, password));
        }

        private void Cancel_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Close(((string?)null, (string?)null));
        }
    }
}
