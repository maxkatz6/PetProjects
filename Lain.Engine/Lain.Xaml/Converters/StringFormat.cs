﻿using System;
using Windows.UI.Xaml.Data;

namespace Lain.Xaml.Converters
{
    public sealed class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            if (parameter == null)
                return value;

            if (value is DateTime)
                return ((DateTime)value).ToString((string)parameter);
            return string.Format((string)parameter, value);
        }
        public object ConvertBack(object value, Type targetType, object parameter,
            string language)
        {
            throw new NotImplementedException();
        }
    }
}