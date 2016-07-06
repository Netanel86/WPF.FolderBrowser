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
        protected IEventAggregator Messanger
        {
            get { return sr_Messanger; }
        }
    }
}
