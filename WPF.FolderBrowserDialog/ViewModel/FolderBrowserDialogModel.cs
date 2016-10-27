using System;
using System.IO;
using System.Windows.Input;
using System.ComponentModel;
using WPF.Common;
using WPF.Common.Aggregators;
using WPF.Common.ViewModel;
using WPF.FolderBrowserDialog.Converters;
using WPF.Common.UI.Behaviors;
using WPF.Common.Services;
using WPF.Common.Messaging;

namespace WPF.FolderBrowserDialog.ViewModel
{
    public class FolderBrowserDialogModel : DialogModel, INavigable
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
                            this.TreeModel.InitiateSearch(m_PathText);
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
                else
                {
                    this.PathText = eStringType.String_MyComputer.GetUnderlyingString();
                }
            }
        }
        
        protected override void Load()
        {
            this.TreeModel.PropertyChanged += onSelectedDirectoryChanged;
            m_Token = Messanger.Subscribe<ErrorMessage>(OnErrorNotice);

            if (this.DefaultValue != null)
            {
                this.TreeModel.InitiateSearch( (this.DefaultValue as PathResult).Path );
            }
        }
        
        protected override void Unload()
        {
            this.TreeModel.PropertyChanged -= onSelectedDirectoryChanged;
            Messanger.Unsubscribe<ErrorMessage>(m_Token);
        }

        protected override void CloseDialog(bool i_DialogCanceled)
        {
            if (!i_DialogCanceled)
            {
                this.ReturnValue = new PathResult() { Path = this.PathText };
            }

            this.Navigator.NavigateBackwards(this.ReturnValue);
        }

        protected override bool CheckResultLegitimacy()
        {
            bool legit = false;

            if (Directory.Exists(PathText) && TreeModel.SelectedItem != null && (TreeModel.SelectedItem as DirectoryModelBase).HasAccess)
            {
                legit = true;
            }

            return legit;
        }

        protected virtual void OnErrorNotice(ErrorMessage i_Message)
        {
            this.Navigator.NavigateTo(i_Message, null);
        }

        #region INavigable
        private INavigationService m_Navigator;
        public INavigationService Navigator
        {
            get
            {
                return m_Navigator;
            }
            set
            {
                m_Navigator = value;
            }
        }
        #endregion
    }
}
