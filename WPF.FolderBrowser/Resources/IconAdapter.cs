using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPF.Common.UI.Converters;
using WPF.FolderBrowser.ViewModel;

namespace WPF.FolderBrowser.Resources
{
    public class IconAdapter : IResourceAdapter
    {
        private const string m_ApplicationPath = @"pack://application:,,,/WPF.FolderBrowser;component/Icons/";

        public object GetResource(object i_ResourceName)
        {
            string resourceString = String.Empty;

            if (i_ResourceName is eIconType)
            {
                switch ((eIconType)i_ResourceName)
                {
                    case eIconType.Find:
                        resourceString = Icons.Find;
                        break;
                    case eIconType.Computer:
                        resourceString = Icons.Computer;
                        break;
                    case eIconType.OpenFolder:
                        resourceString = Icons.OpenFolder;
                        break;
                    case eIconType.ClosedFolder:
                        resourceString = Icons.ClosedFolder;
                        break;
                    case eIconType.NewFolder:
                        resourceString = Icons.NewFolder;
                        break;
                    case eIconType.RenameFolder:
                        resourceString = Icons.RenameFolder;
                        break;
                    case eIconType.NoAccessFolder:
                        resourceString = Icons.NoAccessFolder;
                        break;
                    case eIconType.SimpleDrive:
                        resourceString = Icons.SimpleDrive;
                        break;
                    case eIconType.SystemDrive:
                        resourceString = Icons.SystemDrive;
                        break;
                    case eIconType.NetworkDrive:
                        resourceString = Icons.NetworkDrive;
                        break;
                    case eIconType.RemovableDrive:
                        resourceString = Icons.RemovableDrive;
                        break;
                    case eIconType.ExpanderClosed:
                        resourceString = Icons.ExpanderClosed;
                        break;
                    case eIconType.ExpanderOpen:
                        resourceString = Icons.ExpanderOpen;
                        break;
                    case eIconType.None:
                        resourceString = String.Empty;
                        break;
                }
            }
            else
            {
                resourceString = i_ResourceName as string;
            }


            return m_ApplicationPath + resourceString;
        }
    }
}
