using System;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

using WPF.FolderBrowserDialog.Localization;
using WPF.Common;
using WPF.Common.ViewModel;

namespace WPF.FolderBrowserDialog.ViewModel
{
    public class TreeViewModel : ViewModelBase
    {
        private TreeViewItemModel m_SelectedItem;
        public TreeViewItemModel SelectedItem
        {
            get { return m_SelectedItem; }
            set
            {
                m_SelectedItem = value;
                this.OnPropertyChanged("SelectedItem");
            }
        }
        
        private readonly ObservableCollection<DummyDirectoryModel> r_RootItems;

        private readonly DummyDirectoryModel r_MyComputer;

        public ObservableCollection<DummyDirectoryModel> RootItems
        {
            get { return r_RootItems; }
        }

        public TreeViewModel()
        {
            r_RootItems = new ObservableCollection<DummyDirectoryModel>();
            r_MyComputer = new DummyDirectoryModel(eStringType.String_MyComputer);
            
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType != DriveType.CDRom)
                {
                    DriveModel driveM = new DriveModel(drive, r_MyComputer);
                    r_MyComputer.Children.Add(driveM);
                }
            }
            r_RootItems.Add(r_MyComputer);
        }

        private List<string> concatPathToElements(string i_Path)
        {
            i_Path = i_Path.Replace("/",@"\");
            
            List<string> subpaths = i_Path.Split('\\').ToList();
            subpaths[0] += "\\";
            
            return subpaths;
        }
        
        public void InitiateSearch(string i_Path)
        {
            DirectoryModelBase directory = null;

            //searching initializes only if the path exists.
            if ((directory = TryGetDirectoryItem(i_Path)) != null)
            {
                selectDirectory(directory);
            }
            else
            {
                throw new DirectoryNotFoundException(i_Path);
            }
        }

        public DirectoryModelBase TryGetDirectoryItem(string i_Path)
        {
            DirectoryModelBase directory = null;

            if (Directory.Exists(i_Path))
            {
                List<string> subpaths = concatPathToElements(i_Path);

                directory = findDirectoryTreeItem(subpaths, r_MyComputer);
            }

            return directory;
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

        private void selectDirectory(DirectoryModelBase i_Directory)
        {
            //expend the parent item if its not already expanded
            if (i_Directory.Parent != null && !i_Directory.Parent.IsExpanded)
            {
                i_Directory.Parent.IsExpanded = true;
            }

            i_Directory.IsSelected = true;
            //this.OnPropertyChanged("SelectedItem");
        }
    }
}
