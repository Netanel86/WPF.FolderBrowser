using System;

namespace WPF.Common
{
    /// <summary>
    /// Derived from <see cref="EventArgs"/>, used to pass generic event <typeparamref name="Data"/> to notify
    /// all listeners
    /// </summary>
    /// <typeparam name="T">The type of data to pass</typeparam>
    public class NotificationEventArgs<T> : EventArgs
    {
        /// <summary>
        /// The data to pass
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Initiates a new instance of <see cref="NotificationEventArgs<T>"/>
        /// </summary>
        /// <param name="i_Data">Data to pass</param>
        public NotificationEventArgs(T i_Data)
        {
            this.Data = i_Data;
        }
    }
}
