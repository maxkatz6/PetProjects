using System;
using Windows.UI.Xaml.Navigation;

namespace FSClient.MVVM.NavigationService1
{
    public class NavigatedEventArgs : EventArgs
    {
        public NavigatedEventArgs() { }
        public NavigatedEventArgs(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            PageType = e.SourcePageType;
            Parameter = e.Parameter?.ToString();
            NavigationMode = e.NavigationMode;
        }
        public NavigationMode NavigationMode { get; set; }
        public Type PageType { get; set; }
        public string Parameter { get; set; }
    }
}
