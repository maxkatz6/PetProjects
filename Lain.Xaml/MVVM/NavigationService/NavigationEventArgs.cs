using System;
using Windows.UI.Xaml.Navigation;

namespace FSClient.MVVM.NavigationService1
{
    public class NavigationEventArgs : EventArgs
    {
        public NavigationMode NavigationMode { get; set; }

        public string Parameter { get; set; }
    }
}
