using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using FolderBrowserDialog.Localization;
using FolderBrowserDialog.Controls;
using System.Windows;

namespace FolderBrowserDialog.ViewModel
{
    public interface ISelectedObserver
    {
        void NotifyIsSelected(DirectoryModelBase i_Directory);
    }
    public class DirectoryModelBase : TreeViewItemModel
    {
        private ISelectedObserver m_SelectedObserver;
        private string m_ImagePath = String.Empty;
        public string ImagePath
        {
            get 
            {
                if (m_ImagePath == String.Empty)
                {
                    this.LoadImage();
                }
                return m_ImagePath; 
            }

            set
            {
                if (m_ImagePath.CompareTo(value) != 0)
                {
                    m_ImagePath = value;
                    this.OnPropertyChanged("ImagePath");
                }
            }
        }
        public string FullPath
        {
            get { return m_Directory.FullName; }
        }
        protected DirectoryInfo m_Directory;
        public bool IsEmpty
        {
            get { return m_Directory.GetDirectories().Length == 0; }
        }
        
        private ICommand m_NewFolderCommand;
        public ICommand NewFolderCommand
        {
            get { return m_NewFolderCommand; }
        }
        
        private ICommand m_EnterEditModeCommand;
        public ICommand EnterEditModeCommand
        {
            get { return m_EnterEditModeCommand; }
            set { m_EnterEditModeCommand = value; }
        }

        private eTextControlMode m_CurrentEditMode = eTextControlMode.ReadOnly;
        public eTextControlMode CurrentEditMode
        {
            get { return m_CurrentEditMode; }
            set
            {
                if (m_CurrentEditMode != value)
                {
                    m_CurrentEditMode = value;
                    OnPropertyChanged("CurrentEditMode");
                }

            }
        }
        
        private bool m_HasAccess = true;
        public bool HasAccess
        {
            get
            {
                const bool v_Access = true;
                if (m_HasAccess == v_Access)
                {
                    try
                    {
                        m_HasAccess = m_Directory.GetDirectories() != null ? v_Access : !v_Access;
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        m_HasAccess = !v_Access;
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        m_HasAccess = !v_Access;
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        (this.Parent as DirectoryModelBase).RefreshDirectoryTree();
                        MessageBox.Show(ex.Message + System.Environment.NewLine + Strings.MessegeBoxTextErrorDirectNotFound, Strings.MessegeBoxTitleErrorDirectNotFound, MessageBoxButton.OK);
                    }
                }


                return m_HasAccess;
            }
        }

        private bool m_IsEditable = true;
        public bool IsEditable
        {
            get 
            {
                const bool v_Editable = true;
                return this.HasAccess ? m_IsEditable : !v_Editable; 
            }
            protected set { m_IsEditable = value; }
        }

        public DirectoryModelBase(DirectoryInfo i_Directory, TreeViewItemModel i_ParentDirectory)
            : base(i_ParentDirectory, false)
        {
            CheckIfNull(i_Directory, "i_Directory");
            CheckIfNull(i_ParentDirectory, "i_ParentDirectory");
            m_NewFolderCommand = new RelayCommand(createNewFolder, x => this.HasAccess);
            m_EnterEditModeCommand = new RelayCommand(switchToEditMode, x => this.IsEditable && this.CurrentEditMode == eTextControlMode.ReadOnly);
            m_Directory = i_Directory;

            if (this.HasAccess && !IsEmpty)
            {
                this.Children.Add(this.DummyItem);
            }
        }
        private void switchToEditMode(object obj)
        {
            this.CurrentEditMode = eTextControlMode.Editable;
        }
        private void createNewFolder(object obj)
        {

        }

        protected override void Populate()
        {
            this.Children.Clear();
            if (this.HasAccess && !this.IsEmpty)
            {
                foreach (DirectoryInfo subDirectory in m_Directory.GetDirectories())
                {
                    if ((subDirectory.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        FolderModel folder = new FolderModel(subDirectory, this);
                        folder.AddSelectedObserver(m_SelectedObserver); 
                        this.Children.Add(folder);
                    }
                }
            }
        }

        protected virtual void LoadImage()
        {
        }

        protected void CheckIfNull(object i_Parameter, string i_ParameterName)
        {
            if (i_Parameter == null)
            {
                throw new ArgumentNullException(i_ParameterName);
            }
        }

        public void RefreshDirectoryTree()
        {
            this.Populate();
        }
        
        public void AddSelectedObserver(ISelectedObserver i_Observer)
        {
            this.m_SelectedObserver = i_Observer;
        }
        
        public bool MatchDirectoryName(string i_DirectoryToMatch)
        {
            const bool v_Match = true;
            const bool v_IgnoreCase = true;

            //populate the item before searching deeper.
            if (!this.IsPopulated)
            {
                this.Populate();
            }

            return String.Compare(m_Directory.Name, i_DirectoryToMatch, v_IgnoreCase) == 0 ? v_Match : !v_Match;
        }

        protected override void OnPropertyChanged(string i_Property)
        {
            base.OnPropertyChanged(i_Property);

            if (i_Property.CompareTo("IsSelected") == 0)
            {
                if (this.IsSelected && m_SelectedObserver != null)
                {
                    m_SelectedObserver.NotifyIsSelected(this);
                }
            }
        }
    }
}
