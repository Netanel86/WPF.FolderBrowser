using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WPF.Common;
using System.ComponentModel;

namespace WPF.FolderBrowserDialog.ViewModel
{
    public class FolderBrowserDialogModel : ViewModelBase
    {
        public TreeViewModel TreeModel
        {
            get;
            private set;
        }

        private bool m_IsLoaded = false;

        public ICommand FindDirectoryCommand
        {
            get 
            { 
                return m_FindDirectoryCommand; 
            }
        }

        private ICommand m_FindDirectoryCommand;

        private string m_PathText = String.Empty;

        public string PathText
        {
            get { return m_PathText; }
            set
            {
                m_PathText = value;
                m_FindDirectoryCommand.CanExecute(value);
                this.OnPropertyChanged("PathText");
            }
        }

        public FolderBrowserDialogModel()
        {
            this.TreeModel = new TreeViewModel();
            m_FindDirectoryCommand = new RelayCommand(TreeModel.InitiateSearch, (x) => !String.IsNullOrEmpty(this.PathText));
        }

        public void Initialize()
        {

        }

        private void onSelectedDirectoryChanged(object i_Sender, PropertyChangedEventArgs i_Args)
        {
            if (i_Args.PropertyName.CompareTo("SelectedItem") == 0) 
            { 
                this.PathText = (i_Sender as TreeViewModel).SelectedItem.FullPath; 
            }
        }
        
        public void OnLoaded()
        {
            if (!m_IsLoaded)
            {
                this.TreeModel.PropertyChanged += onSelectedDirectoryChanged;

                m_IsLoaded = true;
            }
        }
        
        public void OnUnloaded()
        {
            if (m_IsLoaded)
            {
                this.TreeModel.PropertyChanged -= onSelectedDirectoryChanged;
                m_IsLoaded = false;
            }
        }
    }
}
