using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.ComponentModel;
using WPF.Common.ViewModel;
using System.Windows.Controls;

namespace WPF.Common.UI.Behaviors
{
    public class WindowLifeCycleBehavior : Behavior<Window>
    {
        public static readonly DependencyProperty HandlerProperty = 
            DependencyProperty.RegisterAttached(
            "Handler",
            typeof(ILifeCycleHandler),
            typeof(WindowLifeCycleBehavior));

        public ILifeCycleHandler Handler
        {
            get { return (ILifeCycleHandler)GetValue(HandlerProperty); }
            set{ SetValue(HandlerProperty,value);}
        }

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.RegisterAttached(
            "CloseCommand",
            typeof(ICommand),
            typeof(WindowLifeCycleBehavior));

        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }
        public static readonly DependencyProperty LoadCommandProperty =
            DependencyProperty.RegisterAttached(
            "LoadCommand",
            typeof(ICommand),
            typeof(WindowLifeCycleBehavior));
        
        public ICommand LoadCommand
        {
            get { return (ICommand)GetValue(LoadCommandProperty); }
            set { SetValue(LoadCommandProperty, value); }
        }
        
        protected override void OnAttached()
        {
            System.Windows.Window window = this.AssociatedObject;

            window.Loaded += onWindowLoaded;
            window.Closing += onWindowClosing;
            window.Closed += onWindowClosed;

            this.Handler.CloseRequest += closeWindow;
            this.Handler.ErrorNotice += notifyError;
            base.OnAttached();
        }

        private void closeWindow(object i_Sender, EventArgs i_Args)
        {
            //to do: check what happens when window in not a dialog
            this.AssociatedObject.DialogResult = !(i_Args as NotificationEventArgs<bool>).Data;
            this.AssociatedObject.Close();
        }

        private void notifyError(object i_Sender, EventArgs i_Args)
        {
            IMessageModel message = (i_Args as NotificationEventArgs<ErrorMessage>).Data;

            MessageBox.Show(
                message.Content + System.Environment.NewLine
                + message.Text,
                message.Title,
                MessageBoxButton.OK,
                MessageBoxImage.Error);


            //System.Diagnostics.Debug.WriteLine(i_Args.Data.Message);
        }

        protected override void OnDetaching()
        {
            System.Windows.Window window = this.AssociatedObject;
           
            window.Loaded -= onWindowLoaded;
            window.Closing -= onWindowClosing;
            window.Closed -= onWindowClosed;

            this.Handler.CloseRequest -= closeWindow;
            this.Handler.ErrorNotice -= notifyError;
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
