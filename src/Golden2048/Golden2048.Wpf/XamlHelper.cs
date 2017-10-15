using System;
using System.Windows;

namespace Golden2048.Wpf
{
    public class XamlHelper
    {
        public static bool GetIsVisible(DependencyObject d) => (bool)d.GetValue(IsVisibleProperty);
        public static void SetIsVisible(DependencyObject d, bool value) => d.SetValue(IsVisibleProperty, value);
        
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.RegisterAttached(
                "IsVisible",
                typeof(bool),
                typeof(XamlHelper),
                new PropertyMetadata(true, IsVisibleChanged)
            );

        private static void IsVisibleChanged(Object sender, DependencyPropertyChangedEventArgs e)
        {
            var elem = sender as UIElement;
            elem.Visibility = (bool)(e.NewValue) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
