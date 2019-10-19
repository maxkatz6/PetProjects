namespace BlockchainNet.View.Gui.Converters
{
    using System;
    using System.Globalization;
    using Avalonia.Data.Converters;

    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is DateTime dateTime
                ? dateTime.ToLocalTime().ToString("[hh:mm:ss]")
                : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
