using System;

namespace WPF.Common
{
    public class NotificationEventArgs<T> : EventArgs
    {
        public T Data { get; set; }

        public NotificationEventArgs(T i_Data)
        {
            this.Data = i_Data;
        }
    }
}
