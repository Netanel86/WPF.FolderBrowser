using System.IO;
using FolderBrowserDialog.Images;
namespace FolderBrowserDialog.ViewModel
{
    public class DriveModel : DirectoryModelBase
    {
        private DriveInfo m_DriveInfo;
        public string DriveLetter 
        {
            get { return m_Directory.Name; }
        }

        public DriveModel(DriveInfo i_DriveInfo, DummyDirectoryModel i_ParentDirectory)
            : base(i_DriveInfo.RootDirectory, i_ParentDirectory)
        {
            this.m_DriveInfo = i_DriveInfo;
            this.IsEditable = false;
        }

        protected override void LoadImage()
        {
            if (System.Environment.SystemDirectory.Contains(m_DriveInfo.Name))
            {
                this.ImagePath = Icons.TreeViewItemDriveSystem;
            }
            else
            {
                this.ImagePath = m_DriveInfo.DriveType == DriveType.Network ? Icons.TreeViewItemDriveNetwork: Icons.TreeViewItemDrive;
            }
        }
    }
}
