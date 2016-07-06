using System;
using System.Reflection;
using System.Windows.Interactivity;
using System.Windows;
using WPF.Common.ViewModel;
using WPF.Common.UI.Infrastracture;

namespace WPF.Common.UI.Behaviors
{
    public class DialogBehaviorManager : Behavior<Window>
    {
        public static readonly DependencyProperty HandlerProperty =
            DependencyProperty.RegisterAttached(
            "Handler",
            typeof(IDialogHandler),
            typeof(DialogBehaviorManager));

        public IDialogHandler Handler
        {
            get { return (IDialogHandler)GetValue(HandlerProperty); }
            set { SetValue(HandlerProperty, value); }
        }

        protected override void OnAttached()
        {
            this.Handler.OpenDialogRequest += openDialog;
        }

        private void openDialog(object i_Sender, EventArgs i_Args)
        {
            CallBackNotificationEventArgs<Type, object> args = i_Args as CallBackNotificationEventArgs<Type, object>;

            Type dialogType = args.Data;

            ConstructorInfo dialogCtor = dialogType.GetConstructor(new Type[0]);

            if (dialogCtor != null)
            {
                DialogBase dialog = dialogCtor.Invoke(new object[0]) as DialogBase;

                if (dialog.ShowDialog() == true && args.Completed != null)
                {
                    args.Completed(dialog.ReturnValue);
                }
            }
        }
    }
}
