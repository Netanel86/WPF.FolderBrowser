using System.IO;
using WPF.FolderBrowserDialog.Images;
namespace WPF.FolderBrowserDialog.ViewModel
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
                this.Icon = eIconType.SystemDrive;
            }
            else
            {
                this.Icon = m_DriveInfo.DriveType == DriveType.Network ? eIconType.NetworkDrive : eIconType.SimpleDrive;
            }
        }

        protected override DirectoryModelBase CreateNewDirectoryModel(DirectoryInfo i_DirectoryInfo, DirectoryModelBase i_Parent)
        {
            return new FolderModel(i_DirectoryInfo, i_Parent);
        }
    }
}
