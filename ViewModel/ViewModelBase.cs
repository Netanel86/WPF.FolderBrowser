﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using FolderBrowserDialog.Localization;
using FolderBrowserDialog.Images;
using FolderBrowserDialog.Common;

namespace FolderBrowserDialog.ViewModel
{
    /// <summary>
    /// A base view model class, which holds application resources
    /// to be accesible threw the view model.
    /// </summary>
    public class ViewModelBase : ObservableObject
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
    }
}
