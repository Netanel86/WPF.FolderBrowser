using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using WPF.Common.ViewModel;
using System.Windows.Controls;
using System.Windows.Interactivity;
using WPF.Common.UI.Infrastracture;
using System.Windows.Input;

namespace WPF.Common.UI.Behaviors
{
    public class LifeCycleBehaviorTest : Behavior<DependencyObject>
    {
        public static readonly DependencyProperty HandlerPropery =
           DependencyProperty.Register("Handler", typeof(ILifeCycleHandler), typeof(LifeCycleBehaviorTest));

        public ILifeCycleHandler Handler
        {
            get { return this.GetValue(HandlerPropery) as ILifeCycleHandler; }
            set { this.SetValue(HandlerPropery, value); }
        }

        public static readonly DependencyProperty CloseCommandProperty = 
            DependencyProperty.RegisterAttached(
            "CloseCommand",
            typeof(ICommand),
            typeof(LifeCycleBehaviorTest));

        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }
        public static readonly DependencyProperty LoadCommandProperty =
            DependencyProperty.RegisterAttached(
            "LoadCommand",
            typeof(ICommand),
            typeof(LifeCycleBehaviorTest));

        public ICommand LoadCommand
        {
            get { return (ICommand)GetValue(LoadCommandProperty); }
            set { SetValue(LoadCommandProperty, value); }
        }

        private DialogUserControl m_Control = null;
        private Window m_Window = null;

        protected override void OnAttached()
        {
            if ((m_Control = this.AssociatedObject as DialogUserControl) != null)
            {
                m_Control.Loaded += onDialogLoaded;
                m_Control.Unloaded += onDialogClosed;

                Handler.CloseRequest += onCloseRequest;
                Handler.ErrorNotice += onMessageEvent;
            }
            else if ((m_Window = this.AssociatedObject as Window) != null)
            {
                //todo
            }

            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            if (m_Control != null)
            {
                m_Control.Loaded -= onDialogLoaded;
                m_Control.Unloaded -= onDialogClosed;

                Handler.CloseRequest -= onCloseRequest;
                Handler.ErrorNotice -= onMessageEvent;
            }
            else if (m_Window != null)
            {
                //todo
            }
            base.OnDetaching();
        }
        
        private void onMessageEvent(object i_Sender, EventArgs i_Args)
        {
            if (m_Control != null)
            {
                IMessageModel message = (i_Args as NotificationEventArgs<ErrorMessage>).Data;
                MessageControl control = new MessageControl();
                control.DataContext = message;
                m_Control.Service.NavigateTo(control, null);
            }
        }
        
        private void onCloseRequest(object i_Sender, EventArgs i_Args)
        {
            if (m_Control != null)
            {
                m_Control.Service.NavigateBackwards(m_Control.ReturnValue);
            }
            else if (m_Window != null)
            {
            }
        }
        private void onDialogLoaded(object i_Sender, EventArgs i_Args)
        {
            if (this.LoadCommand != null && this.LoadCommand.CanExecute(null))
            {
                this.LoadCommand.Execute(null);
            }
        }
        private void onDialogClosed(object i_Sender, EventArgs i_Args)
        {
            if (this.CloseCommand != null && this.CloseCommand.CanExecute(null))
            {
                this.CloseCommand.Execute(null);
            }
        }
    }
}
