using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using FolderBrowserDialog.Localization;
using FolderBrowserDialog.Images;

namespace FolderBrowserDialog.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private Icons m_Icons = new Icons();
        public Icons Icons
        {
            get { return m_Icons; }
        }
        private Strings m_Strings = new Strings();
        public Strings Strings
        {
            get { return m_Strings; }
        }

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
