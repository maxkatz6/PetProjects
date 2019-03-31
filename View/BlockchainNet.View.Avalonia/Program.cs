namespace BlockchainNet.View.Avalonia
{
    using BlockchainNet.View.Avalonia.ViewModels;
    using BlockchainNet.View.Avalonia.Views;
    using global::Avalonia;

    class Program
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainWindow>(() => new MainWindowViewModel());
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI();
    }
}
