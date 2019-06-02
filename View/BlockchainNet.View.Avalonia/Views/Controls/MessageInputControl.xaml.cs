namespace BlockchainNet.View.Gui.Views.Controls
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Input;
    using Avalonia.Markup.Xaml;
    using BlockchainNet.View.Gui.Interfaces;

    public class MessageInputControl : ReactiveUserControl<IMessageInputViewModel>
    {
        public MessageInputControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void MessageInputControl_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (this.FindControl<TextBox>("MessageInputBox") is TextBox textBoxSender)
            {
                ViewModel.SendMessageCommand.Execute(textBoxSender.Text);
                textBoxSender.Text = string.Empty;
            }
        }

        private void MessageInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter
                && sender is TextBox textBoxSender)
            {
                ViewModel.SendMessageCommand.Execute(textBoxSender.Text);
                textBoxSender.Text = string.Empty;
            }
        }
    }
}
