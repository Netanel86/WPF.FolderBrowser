using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using WPF.Common.ViewModel;
using WPF.Common;
using System.Windows.Controls;
using System.Windows.Interactivity;
using WPF.Common.UI.Infrastracture;
using System.Windows.Input;
using System.Reflection;

namespace WPF.Common.UI.Behaviors
{
    public class ViewLifeCycleBehavior : Behavior<DependencyObject>
    {
        public static readonly DependencyProperty HandlerPropery =
           DependencyProperty.Register("Handler", typeof(IClosableElement), typeof(ViewLifeCycleBehavior));

        public IClosableElement Handler
        {
            get { return this.GetValue(HandlerPropery) as IClosableElement; }
            set { this.SetValue(HandlerPropery, value); }
        }

        public static readonly DependencyProperty CloseCommandProperty = 
            DependencyProperty.RegisterAttached(
            "CloseCommand",
            typeof(ICommand),
            typeof(ViewLifeCycleBehavior));

        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }
        public static readonly DependencyProperty LoadCommandProperty =
            DependencyProperty.RegisterAttached(
            "LoadCommand",
            typeof(ICommand),
            typeof(ViewLifeCycleBehavior));

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
            }
            else if ((m_Window = this.AssociatedObject as Window) != null)
            {
                m_Window.Loaded += onDialogLoaded;
                m_Window.Closed += onDialogClosed;

                this.Handler.CloseRequest += onCloseRequest;
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
            }
            else if (m_Window != null)
            {
                m_Window.Loaded -= onDialogLoaded;
                m_Window.Closed -= onDialogClosed;

                this.Handler.CloseRequest -= onCloseRequest;
            }
            base.OnDetaching();
        }
        
        private void onCloseRequest(object i_Sender, EventArgs i_Args)
        {
            if (m_Control != null)
            {
                m_Control.Service.NavigateBackwards(m_Control.ReturnValue);
            }
            else if (m_Window != null)
            {
                m_Window.DialogResult = !(i_Args as NotificationEventArgs<bool>).Data;
                m_Window.Close();
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
