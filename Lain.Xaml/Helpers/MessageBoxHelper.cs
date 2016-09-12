using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace Lain.Xaml.Helpers
{
    public static class MessageBox
    {
        private static bool canShow = true;

        public static async void Show(string message)
        {
            try
            {
                await ShowAsync(message);
            }
            catch { }
        }

        public static async Task ShowAsync(MessageDialog messageDialog)
        {
            try
            {
                if (canShow)
                {
                    canShow = false;
                    await CoreApplication.MainView.Dispatcher.RunAsync(
                        CoreDispatcherPriority.High,
                        async () =>
                        await messageDialog.ShowAsync().AsTask().ContinueWith(
                            r => canShow = true)
                        );
                }
            }
            catch { }
        }
        public static async Task ShowAsync(string message, string title = null)
        {
            await ShowAsync(title != null ? new MessageDialog(message, title) : new MessageDialog(message));
        }
    }
}