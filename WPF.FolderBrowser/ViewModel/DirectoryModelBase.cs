using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using WPF.FolderBrowserDialog.Localization;
using WPF.Common;
using WPF.Common.Enums;
using WPF.Common.ViewModel;
using WPF.FolderBrowserDialog.Resources;
using System.Windows.Data;
using WPF.Common.Messaging;
using WPF.Common.Input;

namespace WPF.FolderBrowserDialog.ViewModel
{

    public abstract class DirectoryModelBase : TreeViewItemModel
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

        public eIconType Icon
        {
            get
            {
                if (m_Icon == eIconType.None)
                {
                    this.LoadImage();
                }
                return m_Icon;
            }

            set
            {
                if (m_Icon.CompareTo(value) != 0)
                {
                    m_Icon = value;
                    this.OnPropertyChanged("ImagePath");
                }
            }
        }

        public abstract string FullPath { get; }

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
                    catch (DirectoryNotFoundException i_Exception)
                    {
                        m_HasAccess = !v_Access;

                        (this.Parent as DirectoryModelBase).RefreshDirectoryTree();
                        
                        Messanger.Publish<ErrorMessage>(
                            new ErrorMessage()
                            {
                                Title = eStringType.ErrorTitle_DirectoryNotFound.GetUnderlyingString(),
                                Text = eStringType.ErrorText_DirectoryNotFound.GetUnderlyingString(),
                                Content = i_Exception.Message,
                                Icon = eMessageIcon.Exclamation
                            });
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

        private eIconType m_Icon = eIconType.None;

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
            string newFolderName = eStringType.String_NewFolderName.GetUnderlyingString();
            string nonExistingNewFolderName = newFolderName;
            int count = 0;
            this.IsExpanded = true;
            //find a "New Folder"/+"no.#" name that doesnt exits in the current directory
            while (Directory.Exists(Path.Combine(m_Directory.FullName, nonExistingNewFolderName)))
            {
                nonExistingNewFolderName = newFolderName + count++.ToString();
            }

       
            //create the new folder and add it to the children collection of the current directory
            DirectoryModelBase subfolder = CreateNewDirectoryModel(m_Directory.CreateSubdirectory(nonExistingNewFolderName), this);
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
            //implement you own image loading logics
        }

        /// <summary>
        /// Initializes a concrete instance of <see cref="DirectoryModelBase"/>
        /// </summary>
        /// <param name="i_DirectoryInfo">Directory to wrap</param>
        /// <param name="i_Parent">Directory parent model</param>
        /// <returns>a new instance of a class extending <see cref="DirectoryModelBase"/></returns>
        /// <remarks>
        /// is called in <code>this.Populate()</code> method, 
        /// implement for creation of concrete extended classes.
        /// </remarks>
        protected abstract DirectoryModelBase CreateNewDirectoryModel(DirectoryInfo i_DirectoryInfo, DirectoryModelBase i_Parent);

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
                        this.Children.Add(folder);
                    }
                }
            }
        }

        public void RefreshDirectoryTree()
        {
            this.Populate();
        }
        
        public bool MatchDirectoryName(string i_DirectoryToMatch)
        {
            const bool v_Match = true;
            const bool v_IgnoreCase = true;

            return String.Compare(m_Directory.Name, i_DirectoryToMatch, v_IgnoreCase) == 0 ? v_Match : !v_Match;
        }
    }
}
