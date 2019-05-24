namespace BlockchainNet.View.Gui
{
    using BlockchainNet.View.Gui.ViewModels;
    using BlockchainNet.View.Gui.Views;
    using Avalonia;
    using BlockchainNet.Messenger;
    using Avalonia.ReactiveUI;
    using System.Text;

    public class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            BuildAvaloniaApp().Start(AppMain, args);
        }

        private static void AppMain(Application app, string[] args)
        {
            var window = new MainWindow
            {
                DataContext = new MainWindowViewModel(new MessengerServiceLocator()),
            };

            _ = app.Run(window);
        }

        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .With(new X11PlatformOptions { UseGpu = false })
                .With(new Win32PlatformOptions { UseDeferredRendering = false })
                .UseReactiveUI();
        }
    }
}
