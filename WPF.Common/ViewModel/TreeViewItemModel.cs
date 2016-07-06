using System.Collections.ObjectModel;
using WPF.Common;

namespace WPF.Common.ViewModel
{
    /// <summary>
    /// A basic <see cref="TreeViewItem"/> view model
    /// </summary>
    /// <remarks>
    /// inherit, use to bind <see cref="TreeViewItem"/> with your own view model.
    /// </remarks>
    public class TreeViewItemModel : ViewModelBase
    {
        #region Members
        private static readonly TreeViewItemModel sr_DummyItem = new TreeViewItemModel();
        
        private readonly ObservableCollection<TreeViewItemModel> r_Children;
        
        private readonly TreeViewItemModel r_Parent;
        
        private bool m_IsExpanded;
        
        private bool m_IsSelected;
        #endregion Members

        #region Properties
        /// <summary>
        /// Dummy Item for managing your own lazy loading logics.
        /// </summary>
        protected TreeViewItemModel DummyItem { get { return sr_DummyItem; } }

        /// <summary>
        /// Collection of children populating the current <typeparamref name="TreeViewItemModel"/>. 
        /// </summary>
        public ObservableCollection<TreeViewItemModel> Children
        {
            get { return r_Children; }
        }

        /// <summary>
        /// The Parent object of the current <typeparamref name="TreeViewItemModel"/>.
        /// </summary>
        public TreeViewItemModel Parent
        {
            get { return r_Parent; }
        }

        /// <summary>
        /// Represents the state of the <paramref name="Children"/> collection.
        /// </summary>
        /// <remarks>
        /// returns true if two criteria's are met: 
        /// 1.the collection is not empty
        /// 2.the first child is not the <see cref="sr_DummyItem"/>
        /// </remarks>
        public bool IsPopulated
        {
            get { return r_Children.Count > 0 && r_Children[0] != sr_DummyItem; }
        }

        /// <summary>
        /// Represents the Expanded state of the <typeparamref name="TreeViewItemModel"/> 
        /// </summary>
        public bool IsExpanded
        {
            get { return m_IsExpanded; }
            set
            {
                const bool v_Expanded = true;
                
                if (value != m_IsExpanded)
                {
                    m_IsExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }
                
                //Unexpand all expanded children - when an ancestor in unexpanded.
                if (!m_IsExpanded && IsPopulated)
                {
                    foreach (TreeViewItemModel child in r_Children)
                    {
                        child.IsExpanded = !v_Expanded;
                    }
                }

                //Expand all unexpanded predeccesors - when an unexpanded child is selected
                if (m_IsExpanded && r_Parent != null)
                {
                    r_Parent.IsExpanded = v_Expanded;
                }

                //Populate if not populated yet
                if (m_IsExpanded && !IsPopulated)
                {
                    this.Populate();
                }
            }
        }

        /// <summary>
        /// Represents the Selected state of the <typeparamref name="TreeViewItemModel"/> 
        /// </summary>
        public bool IsSelected
        {
            get { return m_IsSelected; }
            set
            {
                if (value != m_IsSelected)
                {
                    m_IsSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }
        #endregion Properties

        #region Constructors
        private TreeViewItemModel() { }

        /// <summary>
        /// Initializes a new instance of <typeparamref name="TreeItemViewModel"/>
        /// </summary>
        /// <param name="i_Parent"><typeparamref name="TreeItemViewModel"/> Parent object, null if there is none</param>
        /// <param name="i_AutoLazyLoad">true/false for using implemented lazy load, use false for self implementation</param>
        protected TreeViewItemModel(TreeViewItemModel i_Parent, bool i_AutoLazyLoad)
        {
            r_Parent = i_Parent;
            r_Children = new ObservableCollection<TreeViewItemModel>();
            if (i_AutoLazyLoad)
            {
                r_Children.Add(sr_DummyItem);
            }
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Populates the <paramref name="Children"/> collection with <see cref="TreeItemViewModel"/> type objects.
        /// </summary>
        /// <remarks>
        /// override with you logics to populate the collection only when the <see cref="TreeItemViewModel"/> is expanded.
        /// </remarks>
        protected virtual void Populate()
        {
        }
        #endregion Methods
    }
}
