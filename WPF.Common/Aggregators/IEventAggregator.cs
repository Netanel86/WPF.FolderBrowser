using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF.Common.Aggregators
{
    /// <summary>
    /// An interface for implementing an Event Aggregator. 
    /// Used to pass events and messages between the application layers.
    /// </summary>
    public interface IEventAggregator
    {
        /// <summary>
        /// Publishes a message of <typeparamref name="TMessageType"/> to all subscribers.
        /// </summary>
        /// <typeparam name="TMessageType">The type of message</typeparam>
        /// <param name="i_MessageType">The message</param>
        void Publish<TMessageType>(TMessageType i_Message);

        /// <summary>
        /// Subscribes an action to be invoked when a message of <typeparamref name="TMessageType"/>
        /// is published.
        /// </summary>
        /// <typeparam name="TMessageType">the type of message to subscribe</typeparam>
        /// <param name="i_MethodToInvoke">the action to be invoked</param>
        /// <returns>The subscription between the event aggreagator and the subscriber</returns>
        Subscription<TMessageType> Subscribe<TMessageType>(Action<TMessageType> i_MethodToInvoke);

        /// <summary>
        /// Unsubscribes a <see cref="Subscription<TMessageType>"/> from the event aggregator
        /// </summary>
        /// <typeparam name="TMessageType">the message type of which the subscription is subscribed to</typeparam>
        /// <param name="i_Subscription">the subscription member</param>
        void Unsubscribe<TMessageType>(Subscription<TMessageType> i_Subscription);
    }
}
