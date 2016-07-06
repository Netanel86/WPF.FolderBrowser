using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WPF.Common;
using System.ComponentModel;
using WPF.Common.Aggregators;
using System.Windows;
using WPF.Common.ViewModel;

namespace WPF.FolderBrowserDialog.ViewModel
{
    public class FolderBrowserDialogModel : DialogModel<PathResult>
    {
        private Subscription<Exception> m_Token;
        public event EventHandler<NotificationEventArgs<Exception>> ErrorNotice;
        
        protected void OnErrorNotice(Exception i_Exception)
        {
            if (ErrorNotice != null)
            {
                ErrorNotice.Invoke(this, new NotificationEventArgs<Exception>(i_Exception));
            }
        }
        public TreeViewModel TreeModel
        {
            get;
            private set;
        }

        public ICommand FindDirectoryCommand
        {
            get;
            private set;
        }
        
        private string m_PathText = String.Empty;

        public string PathText
        {
            get { return m_PathText; }
            set
            {
                m_PathText = value;
                this.FindDirectoryCommand.CanExecute(value);
                this.OkCommand.CanExecute(null);
                OnPropertyChanged("PathText");
            }
        }

        public FolderBrowserDialogModel()
            :base()
        {
            this.TreeModel = new TreeViewModel();
            FindDirectoryCommand = new RelayCommand(TreeModel.InitiateSearch, (x) => !String.IsNullOrEmpty(this.PathText));
        }
        
        private void onSelectedDirectoryChanged(object i_Sender, PropertyChangedEventArgs i_Args)
        {
            if (i_Args.PropertyName.CompareTo("SelectedItem") == 0)
            {
                if (TreeModel.SelectedItem is DirectoryModelBase)
                {
                    this.PathText = (TreeModel.SelectedItem as DirectoryModelBase).FullPath;
                }
            }
        }

        protected override void OnClosed()
        {
            this.TreeModel.PropertyChanged -= onSelectedDirectoryChanged;
            Messanger.Unsubscribe<Exception>(m_Token);
        }

        protected override void OnLoaded()
        {
            this.TreeModel.PropertyChanged += onSelectedDirectoryChanged;
            m_Token = Messanger.Subscribe<Exception>(OnErrorNotice);
        }

        protected override void CloseWindow(bool i_DialogCanceled)
        {
            if (!i_DialogCanceled)
            {
                this.ReturnValue = new PathResult() { Path = this.PathText };
            }

            base.CloseWindow(i_DialogCanceled);
        }

        protected override bool CheckResultLegitimacy()
        {
            return Directory.Exists(this.PathText);
        }
    }
}
