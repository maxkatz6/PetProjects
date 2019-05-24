namespace BlockchainNet.View.Gui
{
    using Avalonia;
    using Avalonia.Markup.Xaml;

    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
