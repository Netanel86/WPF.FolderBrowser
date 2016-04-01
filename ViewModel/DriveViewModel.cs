using System;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace FolderBrowserDialog.ViewModel
{
    public class DriveViewModel : BasicDirectoryViewModel
    {
        public string DriveLetter 
        {
            get { return r_Directory.Name; }
        }

        public DriveViewModel(DirectoryTreeViewModel i_Root, DirectoryInfo i_DriveDirectory, DummyDirectoryViewModel i_ParentDirectory)
            : base(i_Root, i_DriveDirectory, i_ParentDirectory)
        {
            this.Image = Properties.Icons.DriveIcon;
            
        }
    }
}
