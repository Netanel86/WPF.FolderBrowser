using System;

namespace WPF.Common
{
    /// <summary>
    /// Extends <see cref="NotificationEventArgs<T>"/>, with a call back method
    /// </summary>
    /// <typeparam name="inT">The type to pass the event</typeparam>
    /// <typeparam name="outT">The type returned when callback method is called</typeparam>
    public class CallBackNotificationEventArgs<inT, outT> : NotificationEventArgs<inT>
    {
        /// <summary>
        /// An action to be invoked when request has completed
        /// </summary>
        public Action<outT> Completed { get; set; }

        /// <summary>
        /// Initiates a new instance of <see cref="CallBackNotificationEventArgs<inT, outT>"/>
        /// </summary>
        /// <param name="i_Data">the data to pass</param>
        /// <param name="i_CompletedAction">an action to be invoked after request is complete</param>
        public CallBackNotificationEventArgs(inT i_Data, Action<outT> i_CompletedAction)
            : base(i_Data)
        {
            this.Completed = i_CompletedAction;
        }
    }
}
