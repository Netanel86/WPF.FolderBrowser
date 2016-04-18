using System;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using FolderBrowserDialog.Localization;
using FolderBrowserDialog.Common;

namespace FolderBrowserDialog.ViewModel
{
    public class TreeViewModel : ViewModelBase , ISelectedDirectoryObserver
    {
        private DirectoryModelBase m_SelectedItem;
        
        private readonly ICommand m_FindDirectoryCommand;
        
        private readonly ObservableCollection<DummyDirectoryModel> r_RootItems;

        private readonly DummyDirectoryModel r_MyComputer;

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

        public ObservableCollection<DummyDirectoryModel> RootItems
        {
            get { return r_RootItems; }
        }

        public ICommand FindDirectoryCommand
        {
            get { return m_FindDirectoryCommand; }
        }

        public TreeViewModel()
        {
            r_RootItems = new ObservableCollection<DummyDirectoryModel>();
            r_MyComputer = new DummyDirectoryModel(Strings.TreeViewItemMyComputer);
            m_FindDirectoryCommand = new RelayCommand(initiateSearch, (x) => !String.IsNullOrEmpty(this.PathText));
            
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType != DriveType.CDRom)
                {
                    DriveModel driveM = new DriveModel(drive, r_MyComputer);
                    driveM.AddSelectedObserver(this as ISelectedDirectoryObserver);
                    r_MyComputer.Children.Add(driveM);
                }
            }
            r_RootItems.Add(r_MyComputer);
        }

        private void initiateSearch(object obj)
        {
            if (Directory.Exists(this.m_PathText))
            {
                string path = Path.GetFullPath(this.PathText);
                List<string> subpaths = path.Split('\\').ToList();
                subpaths[0] += "\\";
                DirectoryModelBase found = findDirectoryTreeItem(subpaths, r_MyComputer);
                if (found.Parent != null && !found.Parent.IsExpanded)
                {
                    found.Parent.IsExpanded = true;
                }
                
                found.IsSelected = true;
            }
            else
            {
                MessageBox.Show(
                    "The specified path does not exists.",
                    "Try Again",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                    );
            }
        }
        
        private DirectoryModelBase findDirectoryTreeItem(List<string> i_PathElements, TreeViewItemModel i_Directory)
        {
            if (i_PathElements.Count != 0)
            {
                if (!i_Directory.IsPopulated)
                {
                    (i_Directory as DirectoryModelBase).RefreshDirectoryTree();
                }

                foreach (DirectoryModelBase subdir in i_Directory.Children)
                {
                    if (subdir.MatchDirectoryName(i_PathElements[0]))
                    {
                        i_PathElements.RemoveAt(0);
                        return findDirectoryTreeItem(i_PathElements, subdir);
                    }
                }
            }

            return i_Directory as DirectoryModelBase;
        }

        public void NotifyIsSelected(DirectoryModelBase i_Directory)
        {
            if (i_Directory.HasAccess)
            {
                this.PathText = i_Directory.FullPath;
            }

            this.m_SelectedItem = i_Directory;
        }
    }
}
