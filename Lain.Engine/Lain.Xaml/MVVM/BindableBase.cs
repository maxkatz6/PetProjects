using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Lain.Xaml.MVVM
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object> objects = new Dictionary<string, object>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected T Get<T>(T def = default(T), [CallerMemberName] string propertyName = null)
        {
            if (!objects.ContainsKey(propertyName))
                objects.Add(propertyName, def);
            return (T)objects[propertyName];
        }
        protected T Get<T>(Func<T> def, [CallerMemberName] string propertyName = null)
        {
            if (!objects.ContainsKey(propertyName))
                objects.Add(propertyName, def());
            return (T)objects[propertyName];
        }
        protected async void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName))
            );
        }
        protected bool Set<T>(T value, bool forceUpdate = false, [CallerMemberName] string propertyName = null)
        {
            if (objects.ContainsKey(propertyName))
            {
                if (!forceUpdate && Equals(objects[propertyName], value))
                    return false;
                objects[propertyName] = value;
            }
            else
                objects.Add(propertyName, value);
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}