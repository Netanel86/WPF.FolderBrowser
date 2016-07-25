using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using WPF.FolderBrowserDialog.ViewModel;
using WPF.FolderBrowserDialog.Localization;

namespace WPF.FolderBrowserDialog.Converters
{
    class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object str = null;
            if (value is eStringType)
            {
                switch ((eStringType)value)
                {
                    case eStringType.Button_Find:
                        str = Strings.ButtonFind;
                        break;
                    case eStringType.String_NewFolderName:
                        str = Strings.NewFolderNameString;
                        break;
                    case eStringType.String_MyComputer:
                        str = Strings.TreeViewItemMyComputer;
                        break;

                    case eStringType.ErrorTitle_DirectoryNotFound:
                        str = Strings.MessegeBoxTitleErrorDirectNotFound;
                        break;
                    case eStringType.ErrorText_DirectoryNotFound:
                        str = Strings.MessegeBoxTextErrorDirectNotFound;
                        break;
                }
            }

            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
