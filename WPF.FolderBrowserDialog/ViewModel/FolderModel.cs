using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using WPF.Common;
using WPF.Common.Enums;
using WPF.FolderBrowserDialog.Resources;
using WPF.FolderBrowserDialog.Localization;
using WPF.FolderBrowserDialog.Converters;

namespace WPF.FolderBrowserDialog.ViewModel
{
    public class FolderModel : DirectoryModelBase
    {
        public override string FullPath
        {
            get { return m_Directory.FullName + @"\"; }
        }

        public string FolderName
        {
            get { return m_Directory.Name; }
        }
        private ICommand m_RenameFolderCommand;
        public ICommand RenameFolderCommand
        {
            get { return m_RenameFolderCommand; }
            set { m_RenameFolderCommand = value; }
        }

        public FolderModel(DirectoryInfo i_Directory, DirectoryModelBase i_ParentDirectory)
            : base(i_Directory, i_ParentDirectory)
        {
            m_RenameFolderCommand = new RelayCommand(renameDirectory);
        }

        protected override void LoadImage()
        {
            this.Icon = this.HasAccess ? eIconType.ClosedFolder : eIconType.NoAccessFolder;
        }

        protected override void OnPropertyChanged(string i_Property)
        {
            base.OnPropertyChanged(i_Property);

            if (this.HasAccess && i_Property.CompareTo("IsExpanded") == 0)
            {
                this.Icon = this.IsExpanded ? eIconType.OpenFolder : eIconType.ClosedFolder;
            }
        }

        private void renameDirectory(object i_NewName)
        {
            try
            {
                string newName = (string)i_NewName;
                if (newName != String.Empty && m_Directory.Name.CompareTo(newName) != 0)
                {
                    if (String.Compare(newName, m_Directory.Name, true) == 0)
                    {
                        string tempName = "temp";
                        m_Directory.MoveTo(Path.Combine(m_Directory.Parent.FullName, tempName));
                    }
                    
                    m_Directory.MoveTo(Path.Combine(m_Directory.Parent.FullName, newName));
                    base.OnPropertyChanged("IsSelected");
                }
            }
            catch (Exception i_Exception)
            {
                this.OnPropertyChanged("FolderName");

                Messanger.Publish<ErrorMessage>(
                    new ErrorMessage()
                    {
                        Title = eStringType.ErrorTitle_Rename.GetUnderlyingString(),
                        Text = eStringType.ErrorText_Rename.GetUnderlyingString(),
                        Content = i_Exception.Message,
                        Icon = eMessageIcon.Warning
                    });
            }
            finally
            {
                this.CurrentEditMode = eTextControlMode.ReadOnly;
            }
        }

        protected override DirectoryModelBase CreateNewDirectoryModel(DirectoryInfo i_DirectoryInfo, DirectoryModelBase i_Parent)
        {
            return new FolderModel(i_DirectoryInfo, i_Parent);
        }
    }
}
