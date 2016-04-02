using System.IO;

namespace FolderBrowserDialog.ViewModel
{
    public class FolderViewModel : BasicDirectoryViewModel
    {
        public string FolderName
        {
            get { return r_Directory.Name; }
        }

        public FolderViewModel(DirectoryTreeViewModel i_Root, DirectoryInfo i_Directory, BasicDirectoryViewModel i_ParentDirectory)
            : base(i_Root, i_Directory, i_ParentDirectory)
        {
            this.Image = Properties.Icons.FolderClosedIcon;
        }

        protected override void OnPropertyChanged(string i_Property)
        {
            base.OnPropertyChanged(i_Property);

            if (i_Property.CompareTo("IsExpanded") == 0)
            {
                this.Image = this.IsExpanded ? Properties.Icons.FolderOpenIcon : Properties.Icons.FolderClosedIcon;
            }
        }
    }
}
