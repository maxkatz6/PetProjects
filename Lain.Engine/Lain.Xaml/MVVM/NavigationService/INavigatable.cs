using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace FSClient.MVVM.NavigationService1
{
    public interface INavigatable
    {
        void OnNavigatedTo(string parameter, NavigationMode mode, Dictionary<string, object> state);
        Task OnNavigatedFromAsync(Dictionary<string, object> state, bool suspending);
        void OnNavigatingFrom(NavigatingEventArgs args);
    }
}
