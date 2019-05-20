namespace BlockchainNet.View.Avalonia
{
    using global::Avalonia;
    using global::Avalonia.Markup.Xaml;

    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
