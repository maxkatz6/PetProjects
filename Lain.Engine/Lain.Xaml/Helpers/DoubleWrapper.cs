﻿namespace Lain.Xaml.Helpers
{
    using Windows.UI.Xaml;
    public class DoubleWrapper : DependencyObject
    {
        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(DoubleWrapper), new PropertyMetadata(0.0));

        public double Value
        {
            get
            {
                return (double)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

    }
}