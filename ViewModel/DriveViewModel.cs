using System.IO;
using FolderBrowserDialog.Images;
namespace FolderBrowserDialog.ViewModel
{
    public class DriveModel : DirectoryModelBase
    {
        private DriveInfo m_DriveInfo;
        public string DriveLetter 
        {
            get { return r_Directory.Name; }
        }

        public DriveModel(TreeViewModel i_Root, DriveInfo i_DriveInfo, DummyDirectoryModel i_ParentDirectory)
            : base(i_Root, i_DriveInfo.RootDirectory, i_ParentDirectory)
        {
            this.m_DriveInfo = i_DriveInfo;
        }

        protected override void LoadImage()
        {
            if (System.Environment.SystemDirectory.Contains(m_DriveInfo.Name))
            {
                this.ImagePath = Icons.TreeViewItemSystemDrive;
            }
            else
            {
                this.ImagePath = m_DriveInfo.DriveType == DriveType.Network ? Icons.TreeViewItemNetworkDrive : Icons.TreeViewItemDrive;
            }
        }
    }
}
