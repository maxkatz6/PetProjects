using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lain.Xaml.MVVM
{
    public class NavigationService
    {
        private static readonly Lazy<NavigationService> Lazy =
            new Lazy<NavigationService>(() => new NavigationService());

        private NavigationService()
        {
        }

        public static NavigationService Instance => Lazy.Value;
        public Frame Frame { get; set; }

        public void GoBack()
        {
            if (Frame == null)
                return;
            if (Frame.CanGoBack)
                Frame.GoBack();
            else if (Frame.BackStackDepth == 0)
                Application.Current.Exit();
        }
        public void GoForward()
        {
            if (Frame == null)
                return;
            if (Frame.CanGoForward)
                Frame.GoForward();
        }
        public bool Navigate<T>(object parameter = null)
        {
            return Navigate(typeof(T), parameter);
        }
        public bool Navigate(Type t, object parameter = null)
        {
            if (Frame == null)
                return false;
            return Frame.Navigate(t, parameter);
        }
    }
    public class NavType
    {
        public NavType()
        {
        }
        public NavType(Type type, string title = null, object par = null)
        {
            Type = type;
            Title = title ?? type.Name;
            Parameter = par;
        }

        public object Parameter { get; set; }
        public string Title { get; set; }
        public Type Type { get; set; }
    }
}