namespace Lain.Xaml.Converters
{
    using System;

    using Windows.UI.Xaml.Data;
    public class BindToObjectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}