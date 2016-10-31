using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPF.Common.UI.Converters;
using WPF.FolderBrowserDialog.ViewModel;

namespace WPF.FolderBrowserDialog.Localization
{
    public class StringAdapter : IResourceAdapter
    {
        public object GetResource(object i_ResourceName)
        {
            string str = String.Empty;

            switch ((eStringType)i_ResourceName)
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

            return str;
        }
    }
}
