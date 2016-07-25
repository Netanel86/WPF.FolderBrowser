using WPF.Common.Aggregators;

namespace WPF.Common.ViewModel
{
    /// <summary>
    /// A base view model class, which holds application Messanger
    /// to interact with view components.
    /// </summary>
    public class ViewModelBase : BindableObject
    {
        private static readonly IEventAggregator sr_Messanger = new EventAggregator();
        
        /// <summary>
        /// Get a static instance of the concrete <see cref="IEventAggregator"/>
        /// </summary>
        protected IEventAggregator Messanger
        {
            get { return sr_Messanger; }
        }
    }

}
