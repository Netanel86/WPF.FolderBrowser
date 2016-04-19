using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Concurrent;

namespace WPF.Common.Aggregators
{
    /// <summary>
    /// An Event Aggregator represents a mediator between a subscriber to a message type,
    /// and a the publisher of that message.
    /// Used to pass events and messages between the application layers.
    /// </summary>
    public class EventAggregator : IEventAggregator
    {
        private readonly object r_LockObject = new object();

        private readonly ConcurrentDictionary<Type, IList> r_Subscriptions;

        /// <summary>
        /// Initializes a new instance of <see cref="EventAggregator"/> class.
        /// </summary>
        public EventAggregator()
        {
            r_Subscriptions = new ConcurrentDictionary<Type, IList>();
        }

        /// <summary>
        /// Publishes a message of <typeparamref name="TMessageType"/> to all subscribers.
        /// </summary>
        /// <typeparam name="TMessageType">The type of message</typeparam>
        /// <param name="i_MessageType">The message</param>
        public void Publish<TMessageType>(TMessageType i_Message)
        {
            Type messageType = typeof(TMessageType);
            IList subscriptions;
            if (r_Subscriptions.ContainsKey(messageType))
            {
                lock (r_LockObject)
                {
                    subscriptions = new List<Subscription<TMessageType>>(r_Subscriptions[messageType].Cast<Subscription<TMessageType>>());
                }

                foreach (Subscription<TMessageType> sub in subscriptions)
                {
                    Action<TMessageType> action = sub.CreateAction();
                    if (action != null)
                    {
                        action(i_Message);
                    }
                }
            }
        }

        /// <summary>
        /// Subscribes an action to be invoked when a message of <typeparamref name="TMessageType"/>
        /// is published.
        /// </summary>
        /// <typeparam name="TMessageType">the type of message to subscribe</typeparam>
        /// <param name="i_MethodToInvoke">the action to be invoked</param>
        /// <returns>The subscription between the event aggreagator and the subscriber</returns>
        public Subscription<TMessageType> Subscribe<TMessageType>(Action<TMessageType> i_MethodToInvoke)
        {
            Type messageType = typeof(TMessageType);
            IList subscriptions;
            Subscription<TMessageType> subscription = new Subscription<TMessageType>(i_MethodToInvoke, this);
            
            lock (r_LockObject)
            {
                //if the message type isnt listed in the dictionary, create a new subscriber's list, 
                //and add a new entry to the dictionary.
                if (!r_Subscriptions.TryGetValue(messageType, out subscriptions))
                {
                    subscriptions = new List<Subscription<TMessageType>>();
                    subscriptions.Add(subscription);
                    r_Subscriptions.GetOrAdd(messageType, subscriptions);
                }
                //if there is already a entry for the message type, add it to its corresponding type list 
                else
                {
                    subscriptions.Add(subscription);
                }
            }

            return subscription;
        }

        /// <summary>
        /// Unsubscribes a <see cref="Subscription<TMessageType>"/> from the event aggregator
        /// </summary>
        /// <typeparam name="TMessageType">the message type of which the subscription is subscribed to</typeparam>
        /// <param name="i_Subscription">the subscription member</param>
        public void Unsubscribe<TMessageType>(Subscription<TMessageType> i_Subscription)
        {
            Type messageType = typeof(TMessageType);
            if (r_Subscriptions.ContainsKey(messageType))
            {
                lock (r_LockObject)
                {
                    r_Subscriptions[messageType].Remove(i_Subscription);
                }

                i_Subscription = null;
            }
        }
    }
}
