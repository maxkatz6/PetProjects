using Avalonia;
using Avalonia.Markup.Xaml;

namespace BlockchainNet.View.Avalonia
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
