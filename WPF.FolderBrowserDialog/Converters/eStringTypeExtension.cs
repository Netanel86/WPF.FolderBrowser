using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using WPF.FolderBrowserDialog.ViewModel;

namespace WPF.FolderBrowserDialog.Converters
{
    public static class eStringTypeExtension
    {
        private static IValueConverter s_StringConverter = new EnumToStringConverter();

        public static string GetUnderlyingString(this eStringType i_String)
        {
            return s_StringConverter.Convert(i_String, typeof(string), null, null) as string;
        }
    }
}
