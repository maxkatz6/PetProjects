using Lain;
using Lain.StoreApp;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Test
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }
        
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (Window.Current.Content == null)
                Window.Current.Content = new Frame();
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.Content == null)
            {
                rootFrame.Height = Config.Height = (int)Window.Current.Bounds.Height;
                rootFrame.Width = Config.Width = (int)Window.Current.Bounds.Width;
                RenderSource source = new RenderSource();
                rootFrame.Content = new Image { Source = source, Stretch = Stretch.Fill };
                var scene = new Scene(source);
            }
            Window.Current.Activate();
        }
    }
}
