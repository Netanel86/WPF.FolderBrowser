namespace FolderBrowserDialog.ViewModel
{
    public class DummyDirectoryViewModel : TreeItemViewModel
    {
        public string Image { get; set; }
        private string m_DummyName;
        public string DummyName
        {
            get { return m_DummyName; }
        }
        public DummyDirectoryViewModel(string i_DummyName)
            : base(null, false)
        {
            this.Image = Properties.Icons.MyComputerIcon;
            m_DummyName = i_DummyName;
        }
    }
}
