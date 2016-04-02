using System.IO;

namespace FolderBrowserDialog.ViewModel
{
    public class DriveViewModel : BasicDirectoryViewModel
    {
        public string DriveLetter 
        {
            get { return r_Directory.Name; }
        }

        public DriveViewModel(DirectoryTreeViewModel i_Root, DriveInfo i_DriveDirectory, DummyDirectoryViewModel i_ParentDirectory)
            : base(i_Root, i_DriveDirectory.RootDirectory, i_ParentDirectory)
        {
            this.Image = i_DriveDirectory.DriveType == DriveType.Network ? Properties.Icons.NetworkDriveIcon : Properties.Icons.DriveIcon;
        }
    }
}
