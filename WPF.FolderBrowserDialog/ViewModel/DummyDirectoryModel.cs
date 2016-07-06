using WPF.FolderBrowserDialog.Images;
using WPF.Common.ViewModel;

namespace WPF.FolderBrowserDialog.ViewModel
{
    public class DummyDirectoryModel : TreeViewItemModel
    {
        public string ImagePath { get; set; }
        private string m_DummyName;
        public string DummyName
        {
            get { return m_DummyName; }
        }
        public DummyDirectoryModel(string i_DummyName)
            : base(null, false)
        {
            this.ImagePath = Icons.TreeViewItemMyComputer;
            m_DummyName = i_DummyName;
        }
    }
}
