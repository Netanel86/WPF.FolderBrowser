using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using WPF.FolderBrowserDialog.ViewModel;
using WPF.FolderBrowserDialog.Resources;

namespace WPF.FolderBrowserDialog.Converters
{
    public class IconToBitmapConverter : IValueConverter
    {
        private const string m_ApplicationPath = @"pack://application:,,,/WPF.FolderBrowserDialog;component/Images/";

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string converted = null;

            if (value is eIconType)
            {
                switch ((eIconType)value)
                {
                    case eIconType.Find:
                        converted = Icons.Find;
                        break;
                    case eIconType.Computer:
                        converted = Icons.Computer;
                        break;
                    case eIconType.OpenFolder:
                        converted = Icons.OpenFolder;
                        break;
                    case eIconType.ClosedFolder:
                        converted = Icons.ClosedFolder;
                        break;
                    case eIconType.NewFolder:
                        converted = Icons.NewFolder;
                        break;
                    case eIconType.RenameFolder:
                        converted = Icons.RenameFolder;
                        break;
                    case eIconType.NoAccessFolder:
                        converted = Icons.NoAccessFolder;
                        break;
                    case eIconType.SimpleDrive:
                        converted = Icons.SimpleDrive;
                        break;
                    case eIconType.SystemDrive:
                        converted = Icons.SystemDrive;
                        break;
                    case eIconType.NetworkDrive:
                        converted = Icons.NetworkDrive;
                        break;
                    case eIconType.ExpanderClosed:
                        converted = Icons.ExpanderClosed;
                        break;
                    case eIconType.ExpanderOpen:
                        converted = Icons.ExpanderOpen;
                        break;
                    case eIconType.None:
                        converted = String.Empty;
                        break;
                }
            }
            else if (value is string)
            {
                converted = value as string;
            }

            return new BitmapImage(new Uri(m_ApplicationPath + converted));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
