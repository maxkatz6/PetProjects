using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Lain.Xaml.Helpers
{
    public static class ItemClickCommand
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof (ICommand),
                typeof (ItemClickCommand), new PropertyMetadata(null, OnCommandPropertyChanged));

        public static ICommand GetCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(CommandProperty);
        }
        public static void SetCommand(DependencyObject d, ICommand value)
        {
            d.SetValue(CommandProperty, value);
        }
        private static void OnCommandPropertyChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var listBase = d as ListViewBase;
            if (listBase != null)
            {
                listBase.ItemClick += OnItemClick;
                return;
            }
            var selectorBase = d as Selector;
            if (selectorBase != null)
                selectorBase.SelectionChanged += OnSelectionChanged;
        }
        private static void OnItemClick(object sender, ItemClickEventArgs e)
        {
            GetCommand(sender as ListViewBase)?.Execute(e.ClickedItem);
        }
        private static void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
                GetCommand(sender as Selector)?.Execute(item);
        }
    }
    public static class SizeBindings
    {
        public static readonly DependencyProperty ActualHeightProperty =
            DependencyProperty.RegisterAttached("ActualHeight", typeof (double), typeof (SizeBindings),
                new PropertyMetadata(0.0));
        public static readonly DependencyProperty ActualWidthProperty =
            DependencyProperty.RegisterAttached("ActualWidth", typeof (double), typeof (SizeBindings),
                new PropertyMetadata(0.0));
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof (bool), typeof (SizeBindings),
                new PropertyMetadata(false, HandlePropertyChanged));

        public static double GetActualHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(ActualHeightProperty);
        }
        public static double GetActualWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(ActualWidthProperty);
        }
        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }
        public static void SetActualHeight(DependencyObject obj, double value)
        {
            obj.SetValue(ActualHeightProperty, value);
        }
        public static void SetActualWidth(DependencyObject obj, double value)
        {
            obj.SetValue(ActualWidthProperty, value);
        }
        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }
        private static void HandlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if (element == null)
                return;

            if ((bool)e.NewValue == false)
                element.SizeChanged -= HandleSizeChanged;
            else
                element.SizeChanged += HandleSizeChanged;
        }
        private static void HandleSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var element = sender as FrameworkElement;

            SetActualHeight(element, e.NewSize.Height);
            SetActualWidth(element, e.NewSize.Width);
        }
    }
    public static class XamlHelper
    {
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null)
                yield break;
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                var typedChild = child as T;
                if (typedChild != null)
                    yield return typedChild;

                foreach (var childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;
            }
        }
    }
}