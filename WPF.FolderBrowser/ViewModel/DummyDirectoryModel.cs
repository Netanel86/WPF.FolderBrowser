using WPF.FolderBrowser.Resources;
using WPF.Common.ViewModel;
using WPF.FolderBrowser.Localization;

namespace WPF.FolderBrowser.ViewModel
{
    public class DummyDirectoryModel : TreeViewItemModel
    {
        public eIconType Icon { get; set; }
        private eStringType m_DummyName;
        public eStringType DummyName
        {
            get { return m_DummyName; }
        }
        public DummyDirectoryModel(eStringType i_DummyName)
            : base(null, false)
        {
            this.Icon = eIconType.Computer;
            m_DummyName = i_DummyName;
        }
    }
}
