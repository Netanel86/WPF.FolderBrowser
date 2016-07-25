using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows;
using WPF.Common.ViewModel;
using System.Windows.Input;
using WPF.Common;
using WPF.Common.UI.Infrastracture;

namespace WPF.Common.UI.Behaviors
{
    public class UserControlLifeCycleBehavior : Behavior<DialogUserControl>
    {
        public static readonly DependencyProperty HandlerProperty =
            DependencyProperty.RegisterAttached(
            "Handler",
            typeof(ILifeCycleHandler),
            typeof(UserControlLifeCycleBehavior));

        public ILifeCycleHandler Handler
        {
            get { return (ILifeCycleHandler)GetValue(HandlerProperty); }
            set { SetValue(HandlerProperty, value); }
        }

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.RegisterAttached(
            "CloseCommand",
            typeof(ICommand),
            typeof(UserControlLifeCycleBehavior));

        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }
        public static readonly DependencyProperty LoadCommandProperty =
            DependencyProperty.RegisterAttached(
            "LoadCommand",
            typeof(ICommand),
            typeof(UserControlLifeCycleBehavior));

        public ICommand LoadCommand
        {
            get { return (ICommand)GetValue(LoadCommandProperty); }
            set { SetValue(LoadCommandProperty, value); }
        }

        protected override void OnAttached()
        {
            UserControl control = this.AssociatedObject;

            control.Loaded += onDialogLoaded;
            control.Unloaded += onDialogUnloaded;
            
            this.Handler.CloseRequest += closeControl;
            this.Handler.ErrorNotice += showMessageControl;
            base.OnAttached();
        }

        private void showMessageControl(object i_Sender, EventArgs i_Args)
        {
            IMessageModel message = (i_Args as NotificationEventArgs<ErrorMessage>).Data;
            MessageControl control = new MessageControl();
            control.DataContext = message;
            this.AssociatedObject.Service.NavigateTo(control, null);
        }

        private void closeControl(object i_Sender, EventArgs i_Args)
        {
            this.AssociatedObject.Service.NavigateBackwards(this.AssociatedObject.ReturnValue);
        }

        protected override void OnDetaching()
        {
            UserControl control = this.AssociatedObject;

            control.Loaded -= onDialogLoaded;
            control.Unloaded -= onDialogUnloaded;

            this.Handler.CloseRequest -= closeControl;
            base.OnDetaching();
        }

        private void onDialogLoaded(object sender, EventArgs e)
        {
            if (LoadCommand != null && LoadCommand.CanExecute(null) )
            {
                LoadCommand.Execute(null);
            }
        }

        private void onDialogUnloaded(object sender, EventArgs e)
        {
            if (CloseCommand != null && CloseCommand.CanExecute(null) )
            {
                CloseCommand.Execute(null);
            }
        }
    }
}
