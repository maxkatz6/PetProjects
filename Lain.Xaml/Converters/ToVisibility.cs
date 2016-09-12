using System;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Lain.Xaml.Converters
{
    public class BooleanConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            var par = (string)parameter;
            bool ret, inv, range;
            range = inv = false;
            if (par != null)
            {
                range = par.Contains("..");
                inv = par.StartsWith("!");
                if (inv)
                    par = par.Substring(1);
            }
            if (!string.IsNullOrEmpty(par))
                if (range)
                {
                    var parts = par.Split(new[] { ".." }, StringSplitOptions.None);
                    var big = !string.IsNullOrWhiteSpace(parts[0]);
                    var low = !string.IsNullOrWhiteSpace(parts[1]);
                    ret = true;
                    if (big)
                    {
                        var left = System.Convert.ChangeType(parts[0], value.GetType());
                        int leftComp = ((IComparable)value).CompareTo(left);
                        ret &= (leftComp >= 0);
                    }
                    if (low && ret)
                    {
                        var right = System.Convert.ChangeType(parts[1], value.GetType());
                        int rightComp = ((IComparable)value).CompareTo(right);
                        ret &= (rightComp <= 0);
                    }
                }
                else
                    ret = value?.ToString() == par;
            else
                if (value is bool)
                ret = (bool)value;
            else if (value is int)
                ret = (int)value != 0;
            else if (value is string)
                ret = !string.IsNullOrEmpty((string)value);
            else if (value is ICollection)
                ret = ((ICollection)value).Count > 0;
            else
                ret = value != null;

            ret ^= inv;

            if (type == typeof(Visibility))
                return ret ? Visibility.Visible : Visibility.Collapsed;
            return ret;
        }
        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            return value;
        }
    }
}