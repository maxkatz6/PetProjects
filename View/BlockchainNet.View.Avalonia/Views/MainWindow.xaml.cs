namespace BlockchainNet.View.Avalonia.Views
{
    using global::Avalonia;
    using global::Avalonia.Controls;
    using global::Avalonia.Markup.Xaml;
    using BlockchainNet.View.Avalonia.ViewModels;

    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
