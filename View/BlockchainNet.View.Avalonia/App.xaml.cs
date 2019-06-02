namespace BlockchainNet.View.Gui
{
    using Avalonia;
    using Avalonia.Markup.Xaml;
    using BlockchainNet.View.Gui.ViewModels;
    using BlockchainNet.View.Gui.Views;
    using BlockchainNet.Messenger;

    public class App : Application
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start(AppMain, args);
        }

        private static void AppMain(Application app, string[] args)
        {
            var window = new MainWindow
            {
                DataContext = new MainWindowViewModel(new MessengerServiceLocator()),
            };

            app.Run(window);
        }

        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .With(new X11PlatformOptions { UseGpu = false })
                .With(new Win32PlatformOptions { UseDeferredRendering = false })
                .UseReactiveUI();
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
