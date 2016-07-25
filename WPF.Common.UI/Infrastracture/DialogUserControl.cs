using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using WPF.Common.UI.Behaviors;

namespace WPF.Common.UI.Infrastracture
{
    /// <summary>
    /// Base class for <see cref="UserControl"/> based dialog's/view's.
    /// </summary>
    /// <remarks>
    /// For use in a single page application, where new controls are displayed ontop 
    /// of the last/current control.
    /// </remarks>
    public class DialogUserControl : UserControl
    {
        public static readonly DependencyProperty ReturnValueProperty =
            DependencyProperty.RegisterAttached(
            "ReturnValue",
            typeof(object),
            typeof(DialogUserControl));

        public object ReturnValue
        {
            get { return GetValue(ReturnValueProperty); }
            set { SetValue(ReturnValueProperty, value); }
        }

        /// <summary>
        /// Get/set the dialog service handler
        /// </summary>
        public IDialogService Service
        {
            get;
            set;
        }
    }
}
