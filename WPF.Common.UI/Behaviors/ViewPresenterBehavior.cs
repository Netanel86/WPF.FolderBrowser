using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows;
using WPF.Common.ViewModel;
using System.Windows.Controls;
using System.Reflection;
using WPF.Common.UI.Infrastracture;
using WPF.Common.UI.View;

namespace WPF.Common.UI.Behaviors
{
    public class ViewPresenterBehavior : Behavior<Panel>, IDialogService
    {
        public static readonly DependencyProperty HandlerPropery =
            DependencyProperty.Register("Handler", typeof(IViewPresenter), typeof(ViewPresenterBehavior));

        public IViewPresenter Handler
        {
            get { return this.GetValue(HandlerPropery) as IViewPresenter; }
            set { this.SetValue(HandlerPropery, value); }
        }

        protected override void OnAttached()
        {
            this.Handler.ShowViewRequest += showView;
            this.Handler.ErrorNotice += showMessage;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            this.Handler.ShowViewRequest -= showView;
            this.Handler.ErrorNotice -= showMessage;
            base.OnDetaching();
        }

        private void showView(object i_Sender, CallBackNotificationEventArgs<Type, object> i_Args)
        {
            ConstructorInfo ctor = i_Args.Data.GetConstructor(new Type[0]);
            if (ctor != null)
            {
                object view = ctor.Invoke(new object[0]);

                if (view is DialogUserControl)
                {
                    NavigateTo(view as DialogUserControl, i_Args.Completed);
                }
                else if (view is DialogBase)
                {
                    DialogBase dialog = view as DialogBase;
                    if (dialog.ShowDialog() == true && i_Args.Completed != null)
                    {
                        i_Args.Completed(dialog.ReturnValue);
                    }
                }
            }
        }

        private void showMessage(object i_Sender, EventArgs i_Args)
        {
            IMessageModel message = (i_Args as NotificationEventArgs<ErrorMessage>).Data;
            MessageControl control = new MessageControl();

            control.DataContext = message;
            control.InitializeComponent();
            NavigateTo(control, null);
        }

        private Stack<Action<object>> m_CallBackFunctions
            = new Stack<Action<object>>();

        public void NavigateTo(DialogUserControl i_Control, Action<object> i_Completed)
        {
            foreach (UIElement element in this.AssociatedObject.Children)
            {
                element.IsEnabled = false;
            }

            i_Control.Service = this as IDialogService;

            this.AssociatedObject.Children.Add(i_Control);
            m_CallBackFunctions.Push(i_Completed);
        }

        public void NavigateBackwards(object i_Result)
        {
            Panel panel = this.AssociatedObject;
            panel.Children.RemoveAt(panel.Children.Count - 1);

            UIElement element = panel.Children[panel.Children.Count - 1];
            element.IsEnabled = true;

            Action<object> completedHandler = m_CallBackFunctions.Pop();
            if (completedHandler != null)
            {
                completedHandler(i_Result);
            }
        }
    }
}
