using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FolderBrowserDialog.ViewModel
{
    public interface ISelectedDirectoryObserver
    {
        void NotifyIsSelected(DirectoryModelBase i_Directory);
    }
}
