namespace Lain.Xaml.Helpers
{
    using System.Collections.Generic;

    using NotificationsExtensions;
    using NotificationsExtensions.Tiles;

    using NotificationsExtensions.Toasts;
    using Windows.ApplicationModel;
    using Windows.UI.Notifications;
    public static class Notifications
    {
        private static readonly TileUpdater Tile = TileUpdateManager.CreateTileUpdaterForApplication();
        private static readonly ToastNotifier Toast = ToastNotificationManager.CreateToastNotifier();

        public static void ShowToast(string line)
        {
            try
            {
                var content = new ToastContent
                {
                    Visual = new ToastVisual
                    {
                        BindingGeneric = new ToastBindingGeneric
                        {
                            Children =
                                            {
                                                new AdaptiveText
                                                    {
                                                        Text = Package.Current.DisplayName
                                                    },
                                                new AdaptiveText
                                                    {
                                                        Text = line
                                                    }
                                            }
                        }
                    }
                };
                var toast = new ToastNotification(content.GetXml());
                Toast.Show(toast);
            }
            catch
            {
            }
        }
        public static void UpdateTile(KeyValuePair<string, string>[] notifications, string displayName = null)
        {
            try
            {
                Tile.Clear();
                Tile.EnableNotificationQueue(true);

                foreach (var pair in notifications)
                {
                    var tile = new TileBindingContentAdaptive
                    {
                        Children =
                                {
                                    new AdaptiveText
                                        {
                                            Text = pair.Key,
                                            HintWrap = true
                                        },
                                    new AdaptiveText
                                        {
                                            Text = pair.Value,
                                            HintStyle = AdaptiveTextStyle.CaptionSubtle
                                        }
                                }
                    };
                    var content = new TileContent
                    {
                        Visual = new TileVisual
                        {
                            DisplayName = displayName,
                            TileMedium = new TileBinding()
                            {
                                Content = tile
                            },
                            TileWide = new TileBinding()
                            {
                                Content =
                                                tile,
                                Branding = TileBranding.NameAndLogo
                            }
                        }
                    };
                    var notification = new TileNotification(content.GetXml());
                    Tile.Update(notification);
                }
            }
            catch
            {
            }
        }
    }
}