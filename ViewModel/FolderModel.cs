using System.IO;
using FolderBrowserDialog.Images;
namespace FolderBrowserDialog.ViewModel
{
    public class FolderModel : DirectoryModelBase
    {
        public string FolderName
        {
            get { return r_Directory.Name; }
        }

        public FolderModel(TreeViewModel i_Root, DirectoryInfo i_Directory, DirectoryModelBase i_ParentDirectory)
            : base(i_Root, i_Directory, i_ParentDirectory)
        {}

        protected override void LoadImage()
        {
            this.ImagePath = this.HasAccess ? Icons.TreeViewItemFolderClosed : Icons.TreeViewItemFolderNoAccess;
        }

        protected override void OnPropertyChanged(string i_Property)
        {
            base.OnPropertyChanged(i_Property);

            if (this.HasAccess && i_Property.CompareTo("IsExpanded") == 0)
            {
                this.ImagePath = this.IsExpanded ? Icons.TreeViewItemFolderOpen : Icons.TreeViewItemFolderClosed;
            }
        }
    }
}
