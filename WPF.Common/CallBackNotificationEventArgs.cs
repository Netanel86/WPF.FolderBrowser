using System;

namespace WPF.Common
{
    public class CallBackNotificationEventArgs<inT, outT> : NotificationEventArgs<inT>
    {
        public Action<outT> Completed { get; set; }

        public CallBackNotificationEventArgs(inT i_Data, Action<outT> i_CompletedAction)
            : base(i_Data)
        {
            this.Completed = i_CompletedAction;
        }
    }
}
