using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FolderBrowserDialog.ViewModel
{
    /// <summary>
    /// A basic <typeparamref name="TreeViewItem"/> view model
    /// </summary>
    /// <remarks>
    /// inherit, use to bind <typeparamref name="TreeViewItem"/> with your own view model.
    /// </remarks>
    public class TreeItemViewModel : INotifyPropertyChanged
    {
        #region Members
        private static readonly TreeItemViewModel sr_DummyItem = new TreeItemViewModel();
        
        private readonly ObservableCollection<TreeItemViewModel> r_Children;
        
        private readonly TreeItemViewModel r_Parent;
        
        private bool m_IsExpanded;
        
        private bool m_IsSelected;
        #endregion Members

        #region Properties
        /// <summary>
        /// Dummy Item for managing your own lazy loading logics.
        /// </summary>
        protected TreeItemViewModel DummyItem { get { return sr_DummyItem; } }

        /// <summary>
        /// Collection of children populating the current <typeparamref name="TreeItemViewModel"/>. 
        /// </summary>
        public ObservableCollection<TreeItemViewModel> Children
        {
            get { return r_Children; }
        }

        /// <summary>
        /// The Parent object of the current <typeparamref name="TreeItemViewModel"/>.
        /// </summary>
        public TreeItemViewModel Parent
        {
            get { return r_Parent; }
        }

        /// <summary>
        /// Represents the state of the <paramref name="Children"/> collection.
        /// </summary>
        public bool IsPopulated
        {
            get { return r_Children.Count > 0 && r_Children[0] != sr_DummyItem; }
        }

        /// <summary>
        /// Represents the Expanded state of the <typeparamref name="TreeItemView"/> 
        /// </summary>
        public bool IsExpanded
        {
            get { return m_IsExpanded; }
            set
            {
                const bool v_Expand = true;
                
                if (value != m_IsExpanded)
                {
                    m_IsExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }
                
                //Unexpand all expanded children
                if (!m_IsExpanded && IsPopulated)
                {
                    foreach (TreeItemViewModel child in r_Children)
                    {
                        child.IsExpanded = !v_Expand;
                    }
                }

                //Expand all unexpanded predeccesors
                if (m_IsExpanded && r_Parent != null)
                {
                    r_Parent.IsExpanded = v_Expand;
                }

                if (m_IsExpanded && !IsPopulated)
                {
                    this.Populate();
                }
            }
        }

        /// <summary>
        /// Represents the Selected state of the <typeparamref name="TreeItemView"/> 
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
        private TreeItemViewModel() { }

        /// <summary>
        /// Initializes a new instance of <typeparamref name="TreeItemViewModel"/>
        /// </summary>
        /// <param name="i_Parent"><typeparamref name="TreeItemViewModel"/> Parent object, null if there is none</param>
        /// <param name="i_AutoLazyLoad">true/false for using implemented lazy load, use false for self implementation</param>
        protected TreeItemViewModel(TreeItemViewModel i_Parent, bool i_AutoLazyLoad)
        {
            r_Parent = i_Parent;
            r_Children = new ObservableCollection<TreeItemViewModel>();
            if (i_AutoLazyLoad)
            {
                r_Children.Add(sr_DummyItem);
            }
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Populates the <paramref name="Children"/> collection with <typeparamref name="TreeItemViewModel"/> type objects.
        /// </summary>
        /// <remarks>
        /// override with you logics to populate the collection only when the <typeparamref name="TreeItemViewModel"/> is expanded.
        /// </remarks>
        protected virtual void Populate()
        {
        }
        #endregion Methods

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Method is called every time a property is changed
        /// </summary>
        /// <param name="i_Property">the name of the updated property</param>
        /// <remarks>
        /// override to add user customized logics whenever a property changes.
        /// </remarks>
        protected virtual void OnPropertyChanged(string i_Property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(i_Property));
            }
        }

        /// <summary>
        /// Notifies when a property is changed
        /// </summary> 
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion INotifyPropertyChanged Members
    }
}
