using System;
using System.IO;
using System.Windows.Input;
using System.ComponentModel;
using WPF.Common;
using WPF.Common.Aggregators;
using WPF.Common.ViewModel;
using WPF.FolderBrowserDialog.Converters;
using WPF.Common.UI.Behaviors;

namespace WPF.FolderBrowserDialog.ViewModel
{
    public class FolderBrowserDialogModel : DialogModel<PathResult>, IViewPresenter
    {
        private Subscription<ErrorMessage> m_Token;
        
        public TreeViewModel TreeModel
        {
            get;
            private set;
        }

        private string m_PathText = String.Empty;

        public int LastVisibleCharIndex
        {
            get { return PathText.Length + 1; }
        }
        
        public string PathText
        {
            get { return m_PathText; }
            set
            {
                const bool v_IgnoreCase = true;
                string oldPath = m_PathText;
                
                m_PathText = value;

                if (m_PathText.EndsWith("/") || m_PathText.EndsWith(@"\"))
                {
                    if (String.Compare(oldPath, m_PathText, v_IgnoreCase) != 0 )
                    {
                        try
                        {
                            this.TreeModel.InitiateSearch(m_PathText as string);
                        }
                        catch(DirectoryNotFoundException i_Execption)
                        {
                            this.OnErrorNotice(
                                new ErrorMessage()
                                {
                                    Title = eStringType.ErrorTitle_DirectoryNotFound.GetUnderlyingString(),
                                    Text = eStringType.ErrorText_DirectoryNotFound.GetUnderlyingString(),
                                    Content = i_Execption.Message,
                                    Icon = eMessageIcon.Warning
                                }
                                );
                        }
                    }
                }
                
                OnPropertyChanged("PathText");

                this.OkCommand.CanExecute(null);
            }
        }

        public FolderBrowserDialogModel()
            :base()
        {
            this.OpenDialogCommand = new RelayCommand(type => OnOpenDialogRequest(type));
            this.TreeModel = new TreeViewModel();
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
        
        protected override void OnLoaded()
        {
            this.TreeModel.PropertyChanged += onSelectedDirectoryChanged;
            m_Token = Messanger.Subscribe<ErrorMessage>(OnErrorNotice);
        }
        
        protected override void OnClosed()
        {
            this.TreeModel.PropertyChanged -= onSelectedDirectoryChanged;
            Messanger.Unsubscribe<ErrorMessage>(m_Token);
        }

        protected override void OnCloseRequest(bool i_DialogCanceled)
        {
            if (!i_DialogCanceled)
            {
                this.ReturnValue = new PathResult() { Path = this.PathText };
            }

            base.OnCloseRequest(i_DialogCanceled);
        }

        protected override bool CheckResultLegitimacy()
        {
            bool legit = false;

            if (TreeModel.SelectedItem != null && (TreeModel.SelectedItem as DirectoryModelBase).HasAccess)
            {
                legit = true;
            }

            return legit;
        }

        protected void OnOpenDialogRequest(object i_DialogType)
        {
            if (this.ShowViewRequest != null)
            {
                this.ShowViewRequest(this, new CallBackNotificationEventArgs<Type, object>(i_DialogType as Type, null));
            }
        }

        public ICommand OpenDialogCommand
        {
            get;
            private set;
        }

        protected virtual void OnErrorNotice(ErrorMessage i_Message)
        {
            if (this.ErrorNotice != null)
            {
                ErrorNotice(this, new NotificationEventArgs<ErrorMessage>(i_Message));
            }
        }

        public event EventHandler<CallBackNotificationEventArgs<Type, object>> ShowViewRequest;

        public event EventHandler ErrorNotice;
    }
}
