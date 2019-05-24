namespace BlockchainNet.View.Gui.Views.Dialogs
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;

    public class InputDialog : Window
    {
        public InputDialog()
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
            var login = this.FindControl<TextBox>("InputTextBox")?.Text;
            Close(login);
        }

        private void Cancel_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            Close(null);
        }
    }
}
