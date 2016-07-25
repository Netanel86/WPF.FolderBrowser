using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows;
using WPF.Common.ViewModel;
using WPF.Common;
using System.Reflection;
using WPF.Common.UI.Infrastracture;
using System.Windows.Controls;

namespace WPF.Common.UI.Behaviors
{
    public class UserControlDialogPresenter : Behavior<Panel>, IDialogService
    {
        public static readonly DependencyProperty HandlerProperty =
            DependencyProperty.RegisterAttached(
            "Handler",
            typeof(IDialogPresenter),
            typeof(UserControlDialogPresenter));

        public IDialogPresenter Handler
        {
            get { return (IDialogPresenter)GetValue(HandlerProperty); }
            set { SetValue(HandlerProperty, value); }
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

        private void openUserControlDialog(object i_Sender, EventArgs i_Args)
        {
            CallBackNotificationEventArgs<Type, object> args = i_Args as CallBackNotificationEventArgs<Type, object>;
            Type dialogType = args.Data;

            ConstructorInfo usercontrolCtor = dialogType.GetConstructor(new Type[0]);

            if (usercontrolCtor != null)
            {
                NavigateTo(usercontrolCtor.Invoke(new object[0]) as DialogUserControl, args.Completed);
            }

        }

        protected override void OnAttached()
        {
            this.Handler.OpenDialogRequest += openUserControlDialog;
        }

        protected override void OnDetaching()
        {
            this.Handler.OpenDialogRequest -= openUserControlDialog;
        }
    }
}
