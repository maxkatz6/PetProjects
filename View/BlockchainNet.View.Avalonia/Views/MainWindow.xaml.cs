using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BlockchainNet.View.Avalonia.ViewModels;

namespace BlockchainNet.View.Avalonia.Views
{
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
