using System;
using System.Reflection;
using WPF.Common;
namespace WPF.Common.Aggregators
{
    /// <summary>
    /// A Subscription class represent's a contract between a subscriber, 
    /// and the event aggregator (ie. the publisher).
    /// </summary>
    /// <typeparam name="TMessageType">The type of message you wish to susbscribe for</typeparam>
    /// <remarks>the subscription instance is to be initiated in the aggregator class</remarks>
    public class Subscription<TMessageType> : DisposableObject
    {
        private readonly IEventAggregator r_EventAggregator;
        private readonly MethodInfo r_MethodInfo;
        private readonly WeakReference r_TargetObject;
        private readonly bool r_IsStatic;
        private bool m_IsDisposed;

        /// <summary>
        /// Initializes an instance of <see cref="Subscription<TMessageType>"/> class
        /// </summary>
        /// <param name="i_MethodToInvoke">Method to be called when a message of type <typeparamref name="TMessageType"/> is published</param>
        /// <param name="i_Aggregator">The aggregator to which you are subscribing to</param>
        public Subscription(Action<TMessageType> i_MethodToInvoke, IEventAggregator i_Aggregator)
        {
            r_MethodInfo = i_MethodToInvoke.Method;

            if (i_MethodToInvoke.Target == null)
            {
                this.r_IsStatic = true;
            }

            r_TargetObject = new WeakReference(i_MethodToInvoke.Target);
            r_EventAggregator = i_Aggregator;
        }

        /// <summary>
        /// Creates the action to be invoked when a message of type 
        /// <typeparamref name="TMessageType"/> has been published.
        /// </summary>
        /// <returns>A refrence to the action, if its target is alive/is static, null otherwise</returns>
        public Action<TMessageType> CreateAction()
        {
            Action<TMessageType> action = null;

            //if the target subscriber still exits and alive
            if (r_TargetObject.Target != null && r_TargetObject.IsAlive)
            {
                action = (Action<TMessageType>)Delegate.CreateDelegate(typeof(Action<TMessageType>), r_TargetObject.Target, r_MethodInfo);
            }

            //if the target subscriber is null, check if subscribed with a static method
            if (this.r_IsStatic)
            {
                action = (Action<TMessageType>)Delegate.CreateDelegate(typeof(Action<TMessageType>), r_MethodInfo);
            }

            return action;
        }

        public override string ToString()
        {
            return String.Format(@"Contract Info: Method Name: {0} 
              TargetObject: {1}
               MessageType: {2}
            Hash Idetifier: [{3:x8}]"
                , r_MethodInfo.Name, r_TargetObject.Target.ToString(), typeof(TMessageType), this.GetHashCode());
        }

        /// <summary>
        /// Unregisters the subscription from the Aggregator and disposes the object.
        /// </summary>
        protected override void Dispose(bool i_IsExpilicitCall)
        {
            if (!m_IsDisposed)
            {
                if (i_IsExpilicitCall)
                {
                    //free class managed objects
                    r_EventAggregator.Unsubscribe(this);
                }

                m_IsDisposed = true;
            }

            base.Dispose(i_IsExpilicitCall);
        }
    }
}
