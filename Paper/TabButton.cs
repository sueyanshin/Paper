using System.Windows;
using System.Windows.Controls;

namespace Paper
{
    public class TabButton : Button
    {
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(TabButton), new PropertyMetadata(false));

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }
    }
}