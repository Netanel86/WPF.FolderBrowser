using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF.FolderBrowserDialog.ViewModel
{
    public interface ISelectedDirectoryObserver
    {
        void NotifySelectedItemChanged(DirectoryModelBase i_Directory);
    }
}
