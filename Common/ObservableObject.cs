﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FolderBrowserDialog.Common
{
    /// <summary>
    /// Class implementing <see cref="INotifyPropertyChanged"/> for use
    /// in observable objects to notify when a property on that object has changed.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
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
