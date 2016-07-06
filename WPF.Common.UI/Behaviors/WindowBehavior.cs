using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.ComponentModel;
using WPF.Common.ViewModel;

namespace WPF.Common.UI.Behaviors
{
    public class WindowBehavior : Behavior<Window>
    {
        public static readonly DependencyProperty HandlerProperty = 
            DependencyProperty.RegisterAttached(
            "Handler",
            typeof(IWindowHandler),
            typeof(WindowBehavior));

        public IWindowHandler Handler
        {
            get { return (IWindowHandler)GetValue(HandlerProperty); }
            set{ SetValue(HandlerProperty,value);}
        }

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.RegisterAttached(
            "CloseCommand",
            typeof(ICommand),
            typeof(WindowBehavior));

        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }
        public static readonly DependencyProperty LoadCommandProperty =
            DependencyProperty.RegisterAttached(
            "LoadCommand",
            typeof(ICommand),
            typeof(WindowBehavior));
        
        public ICommand LoadCommand
        {
            get { return (ICommand)GetValue(LoadCommandProperty); }
            set { SetValue(LoadCommandProperty, value); }
        }
        
        protected override void OnAttached()
        {
            Window window = this.AssociatedObject;

            window.Loaded += onWindowLoaded;
            window.Closing += onWindowClosing;
            window.Closed += onWindowClosed;

            this.Handler.CloseWindowRequest += closeWindow;
            base.OnAttached();
        }

        private void closeWindow(object i_Sender, EventArgs i_Args)
        {
            //to do: check what happens when window in not a dialog
            this.AssociatedObject.DialogResult = !(i_Args as NotificationEventArgs<bool>).Data;
            this.AssociatedObject.Close();
        }

        protected override void OnDetaching()
        {
            Window window = this.AssociatedObject;
           
            window.Loaded -= onWindowLoaded;
            window.Closing -= onWindowClosing;
            window.Closed -= onWindowClosed;

            this.Handler.CloseWindowRequest -= closeWindow;
            base.OnDetaching();
        }

        private void onWindowLoaded(object sender, EventArgs e)
        {
            if (LoadCommand != null)
            {
                LoadCommand.Execute(null);
            }
        }

        private void onWindowClosed(object sender, EventArgs e)
        {
            if (CloseCommand != null)
            {
                CloseCommand.Execute(null);
            }
        }

        private void onWindowClosing(object sender, CancelEventArgs e)
        {
            bool cancel = false;
            if (CloseCommand != null)
            {
                cancel = !CloseCommand.CanExecute(null);
            }
            
            e.Cancel = cancel;
        }
    }
}
