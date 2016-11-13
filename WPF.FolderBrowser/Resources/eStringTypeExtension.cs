using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using WPF.FolderBrowserDialog.ViewModel;
using WPF.Common.UI.Converters;
using WPF.FolderBrowserDialog.Localization;

namespace WPF.FolderBrowserDialog.Resources
{
    public static class eStringTypeExtension
    {
        private static readonly IValueConverter sr_StringConverter = new GeneralStringConverter();
        private static readonly IResourceAdapter sr_StringAdapter = new StringAdapter();

        public static string GetUnderlyingString(this eStringType i_String)
        {
            return sr_StringConverter.Convert(i_String, typeof(string), sr_StringAdapter, null) as string;
        }
    }
}
