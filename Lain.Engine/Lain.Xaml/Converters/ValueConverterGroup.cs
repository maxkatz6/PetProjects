namespace Lain.Xaml.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;
    public class ValueConverterGroup : List<IValueConverter>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return this.Aggregate(value, (current, converter) =>
                 current == DependencyProperty.UnsetValue ?
                 current :
                 converter.Convert(current, targetType, parameter, language));
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return this.AsEnumerable().Reverse().Aggregate(
                value,
                (current, converter) =>
                current == DependencyProperty.UnsetValue
                    ? current
                    : converter.ConvertBack(current, targetType, parameter, language));
        }
    }
}