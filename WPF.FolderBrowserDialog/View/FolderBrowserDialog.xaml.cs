using System.Windows;
using System.Windows.Controls;
using WPF.FolderBrowserDialog.ViewModel;
using System.Windows.Media;

namespace WPF.FolderBrowserDialog.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FolderBrowserDialog : Window
    {
        private FolderBrowserDialogModel DialogModel
        {
            get { return Resources["DialogModel"] as FolderBrowserDialogModel; }
        }

        public FolderBrowserDialog()
        {
            InitializeComponent();
            DialogModel.Initialize();
            this.Loaded += delegate { DialogModel.OnLoaded(); };
            this.Unloaded += delegate { DialogModel.OnUnloaded(); };
        }
    }
}
