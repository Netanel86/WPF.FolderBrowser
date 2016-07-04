using System;

namespace WPF.Common
{
    /// <summary>
    /// A Disposable object base, for classes that needs to, or can be disposed during runtime.
    /// </summary>
    /// <remarks>
    /// use the <code>using( myDisposableObject )</code> statement for explicit dispose.
    /// </remarks>
    public abstract class DisposableObject : IDisposable
    {
        #region Fields
        private bool m_IsDisposed = false;
        #endregion Fields

        #region Finalize
        ~DisposableObject()
        {
            Console.WriteLine("Finalizing: {0:x8}", this.GetHashCode());
            Dispose(false);
        }
        #endregion Finalize

        #region Methods
        /// <summary>
        /// Call to dispose of the object on user demand.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected virtual methd for disposing your own resources on an inherited class. 
        /// </summary>
        /// <param name="i_IsExpilicitCall">represent whether the dispose method was called by the user or the <see cref="GC"/></param>
        /// <remarks>When overriding be sure to call <code>base.Dispose(i_IsExpilicitCall)</code></remarks>
        protected virtual void Dispose(bool i_IsExpilicitCall)
        {
            if (!m_IsDisposed)
            {
                if (i_IsExpilicitCall)
                {
                    //free class member managed objects
                    Console.WriteLine("Dispose called on:\n{0}", this);
                    Console.WriteLine("Explicit call Dispose(true): {0:x8}\n", this.GetHashCode());
                }
                else
                {
                    Console.WriteLine("Implicit call Dispose(false): {0:x8}\n", this.GetHashCode());
                }

                //free unmanaged objects (resources from base class which are initiated in the derived class)

                m_IsDisposed = true;
            }
        }
        #endregion Methods
    }
}
