using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Threading;

namespace FolderBrowserDialog.ViewModel
{
        public class DirectoryTreeViewModel : INotifyPropertyChanged
    {
        private readonly ICommand m_FindDirectoryCommand;
        
        private readonly ObservableCollection<DummyDirectoryViewModel> r_RootItems;

        private readonly DummyDirectoryViewModel r_MyComputer;

        private string m_PathText = String.Empty;

        public string PathText 
        {
            get { return m_PathText; }
            set 
            { 
                m_PathText = value;
                m_FindDirectoryCommand.CanExecute(value);
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("PathText"));
            }
        }

        public ObservableCollection<DummyDirectoryViewModel> RootItems
        {
            get { return r_RootItems; }
        }

        public ICommand FindDirectoryCommand
        {
            get { return m_FindDirectoryCommand; }
        }
        
        public DirectoryTreeViewModel(DriveInfo[] i_SystemDrives)
        {
            r_RootItems = new ObservableCollection<DummyDirectoryViewModel>();
            r_MyComputer = new DummyDirectoryViewModel("My Computer");
            m_FindDirectoryCommand = new RelayCommand(initiateSearch, canStartSearch);
            
            foreach (DriveInfo drive in i_SystemDrives)
            {
                if (drive.DriveType != DriveType.CDRom)
                {
                    r_MyComputer.Children.Add(new DriveViewModel(this, drive.RootDirectory, r_MyComputer));
                }
            }

            r_RootItems.Add(r_MyComputer);
        }

        private bool canStartSearch(object obj)
        {
            return this.PathText != String.Empty;
        }
        private void initiateSearch(object obj)
        {
            if (Directory.Exists(this.m_PathText))
            {
                string path = Path.GetFullPath(this.PathText);
                List<string> subpaths = path.Split('\\').ToList();
                subpaths[0] += "\\";
                BasicDirectoryViewModel found = findDirectoryTreeItem(subpaths, r_MyComputer);
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
        private BasicDirectoryViewModel findDirectoryTreeItem(List<string> i_PathElements, TreeItemViewModel i_Directory)
        {
            if (i_PathElements.Count != 0)
            {
                foreach (BasicDirectoryViewModel subdir in i_Directory.Children)
                {
                    if (subdir.MatchDirectoryName(i_PathElements[0]))
                    {
                        i_PathElements.RemoveAt(0);
                        return findDirectoryTreeItem(i_PathElements, subdir);
                    }
                }
            }

            return i_Directory as BasicDirectoryViewModel;
        }
 
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(sender, args);
            }
        }
    }
}
