using System.Windows;

namespace WPF.Common.UI.Infrastracture
{
    public abstract class DialogBase : Window
    {
        public static readonly DependencyProperty ReturnValueProperty =
            DependencyProperty.RegisterAttached(
            "ReturnValue",
            typeof(object),
            typeof(DialogBase));

        public object ReturnValue
        {
            get { return GetValue(ReturnValueProperty); }
            set { SetValue(ReturnValueProperty, value); }
        }
    }
}
