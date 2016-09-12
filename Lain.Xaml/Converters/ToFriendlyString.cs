using System;
using System.Linq;
using Lain.Xaml.Helpers;
using Windows.UI.Xaml.Data;

namespace Lain.Xaml.Converters
{
    public class ToFriendlyStringConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            if (value == null)
                return string.Empty;
            if (value is string)
            {
                var s = (string)value;
                return string.Join(" ", s.Split('.', '_', '+'));
            }
            if (value is Array)
            {
                var arr = (string[])value;
                int l;
                return string.Join(", ", int.TryParse((string)parameter, out l) ? arr.Take(l) : arr);
            }
            if (value is Enum)
                return ((Enum)value).GetDisplayName() ?? value.ToString();
            return value.ToString();
        }
        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            if (type == typeof(string))
                return string.Join(".", ((string)value).Split(' '));
            if (type == typeof(Array))
                return ((string)value).Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            return value;
        }
    }
}