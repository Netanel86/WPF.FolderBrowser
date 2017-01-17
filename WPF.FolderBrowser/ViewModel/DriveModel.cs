using System.IO;
using WPF.FolderBrowser.Resources;
using System;
namespace WPF.FolderBrowser.ViewModel
{
    public class DriveModel : DirectoryModelBase
    {
        public override string FullPath
        {
            get { return m_Directory.FullName; }
        }
        
        private DriveInfo m_DriveInfo;
        
        public string DriveLetter 
        {
            get { return String.Format("{0} ({1})", m_DriveInfo.VolumeLabel, m_Directory.Name); }
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
                switch (m_DriveInfo.DriveType)
                {
                    case DriveType.Network:
                        this.Icon = eIconType.NetworkDrive;
                        break;
                    case DriveType.Removable:
                        this.Icon = eIconType.RemovableDrive;
                        break;
                    default:
                        this.Icon = eIconType.SimpleDrive;
                        break;
                }
            }
        }

        protected override DirectoryModelBase CreateNewDirectoryModel(DirectoryInfo i_DirectoryInfo, DirectoryModelBase i_Parent)
        {
            return new FolderModel(i_DirectoryInfo, i_Parent);
        }
    }
}
