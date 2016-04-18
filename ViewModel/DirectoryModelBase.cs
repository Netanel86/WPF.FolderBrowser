using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using FolderBrowserDialog.Localization;
using FolderBrowserDialog.Controls;
using FolderBrowserDialog.Common;

namespace FolderBrowserDialog.ViewModel
{

    public class DirectoryModelBase : TreeViewItemModel
    {
        public DirectoryModelBase(DirectoryInfo i_Directory, TreeViewItemModel i_ParentDirectory)
            : base(i_ParentDirectory, false)
        {
            CheckIfNull(i_Directory, "i_Directory");
            CheckIfNull(i_ParentDirectory, "i_ParentDirectory");

            m_NewFolderCommand = new RelayCommand(createNewFolder, x => this.HasAccess);
            m_EnterEditModeCommand =
                new RelayCommand(switchToEditMode, x => this.IsEditable && this.CurrentEditMode == eTextControlMode.ReadOnly);

            m_Directory = i_Directory;

            if (this.HasAccess && !IsEmpty)
            {
                this.Children.Add(this.DummyItem);
            }
        }

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
                        MessageBox.Show(ex.Message + System.Environment.NewLine + Strings.MessegeBoxTextErrorDirectNotFound, Strings.MessegeBoxTitleErrorDirectNotFound, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }


                return m_HasAccess;
            }
        }

        public bool IsEmpty
        {
            get { return m_Directory.GetDirectories().Length == 0; }
        }

        public bool IsEditable
        {
            get
            {
                const bool v_Editable = true;
                return this.HasAccess ? m_IsEditable : !v_Editable;
            }
            protected set { m_IsEditable = value; }
        }

        public ICommand NewFolderCommand
        {
            get { return m_NewFolderCommand; }
        }

        public ICommand EnterEditModeCommand
        {
            get { return m_EnterEditModeCommand; }
        }

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

        protected DirectoryInfo m_Directory;

        private ISelectedDirectoryObserver m_SelectedObserver;
        
        private string m_ImagePath = String.Empty;

        private bool m_HasAccess = true;

        private bool m_IsEditable = true;

        private ICommand m_NewFolderCommand;
        
        private ICommand m_EnterEditModeCommand;
       
        private eTextControlMode m_CurrentEditMode = eTextControlMode.ReadOnly;
        
        private void switchToEditMode(object obj)
        {
            this.CurrentEditMode = eTextControlMode.Editable;
        }
        
        private void createNewFolder(object obj)
        {
            string nonExistingNewFolderName = Strings.NewFolderNameString;
            int count = 0;
            
            //find a "New Folder"/+"#" name that doesnt exits in the current directory
            while (Directory.Exists(Path.Combine(m_Directory.FullName, nonExistingNewFolderName)))
            {
                count++;
                nonExistingNewFolderName = Strings.NewFolderNameString + count.ToString();
            }
            
            //create the new folder and add it to the children collection of the current directory
            DirectoryModelBase subfolder = CreateNewDirectoryModel(m_Directory.CreateSubdirectory(nonExistingNewFolderName), this);
            subfolder.AddSelectedObserver(m_SelectedObserver);
            this.Children.Add(subfolder);
            
            //set the newly created folder as the selected tree item and enter edit mode
            subfolder.IsSelected = true;
            subfolder.EnterEditModeCommand.Execute(null);
        }

        protected void CheckIfNull(object i_Parameter, string i_ParameterName)
        {
            if (i_Parameter == null)
            {
                throw new ArgumentNullException(i_ParameterName);
            }
        }

        protected virtual void LoadImage()
        {
        }

        /// <summary>
        /// Initializes an instance of <typeparamref name="DirectoryModelBase"/>
        /// </summary>
        /// <param name="i_DirectoryInfo">Directory to wrap</param>
        /// <param name="i_Parent">Directory parent model</param>
        /// <returns>a new instance of a class extending <typeparamref name="DirectoryModelBase"/></returns>
        /// <remarks>
        /// is called in <code>this.Populate()</code> method, 
        /// override to implement creation of concrete extended classes.
        /// </remarks>
        protected virtual DirectoryModelBase CreateNewDirectoryModel(DirectoryInfo i_DirectoryInfo, DirectoryModelBase i_Parent)
        {
            throw new NotImplementedException();
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
                        DirectoryModelBase folder = CreateNewDirectoryModel(subDirectory, this);
                        folder.AddSelectedObserver(m_SelectedObserver);
                        this.Children.Add(folder);
                    }
                }
            }
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
        
        public void RefreshDirectoryTree()
        {
            this.Populate();
        }
        
        public void AddSelectedObserver(ISelectedDirectoryObserver i_Observer)
        {
            this.m_SelectedObserver = i_Observer;
        }
        
        public bool MatchDirectoryName(string i_DirectoryToMatch)
        {
            const bool v_Match = true;
            const bool v_IgnoreCase = true;

            return String.Compare(m_Directory.Name, i_DirectoryToMatch, v_IgnoreCase) == 0 ? v_Match : !v_Match;
        }
    }
}
