using System;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using WPF.FolderBrowserDialog.Localization;
using WPF.Common;

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
            r_MyComputer = new DummyDirectoryModel(Strings.TreeViewItemMyComputer);
            
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

        public void InitiateSearch(object i_Parameter)
        {
            string text = (string)i_Parameter;

            //searching initializes only if the path exists.
            if (Directory.Exists(text))
            {
                string path = Path.GetFullPath(text);
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
                FolderBrowserDialogModel.Messanger.Publish<Exception>(new DirectoryNotFoundException());
                //MessageBox.Show(
                //    "The specified path does not exists.",
                //    "Try Again",
                //    MessageBoxButton.OK,
                //    MessageBoxImage.Information
                //    );
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
    }
}
