using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Lain.Xaml.Helpers
{
    public static class ListHelper
    {
        public static void AddRange<T>(this ObservableCollection<T> list, IEnumerable<T> items)
        {
            foreach (var i in items)
                list.Add(i);
        }

        public static async Task SafeAddRangeAsync<T>(this ObservableCollection<T> list, IEnumerable<T> items)
        {
            await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.High, () => AddRange(list, items));
        }
        public static async Task SafeClearAsync<T>(this ObservableCollection<T> list)
        {
            await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.High, list.Clear);
        }
    }
}
