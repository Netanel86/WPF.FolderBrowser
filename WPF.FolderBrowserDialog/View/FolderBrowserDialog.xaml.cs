using System.Windows;
using System.Windows.Controls;
using WPF.FolderBrowserDialog.ViewModel;
using System.Windows.Media;
using WPF.FolderBrowserDialog.Localization;
using System;
using WPF.Common;
using WPF.Common.UI.Infrastracture;
using System.Windows.Data;

namespace WPF.FolderBrowserDialog.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FolderBrowserDialog : DialogBase
    {
        private FolderBrowserDialogModel DialogModel
        {
            get { return Resources["DialogModel"] as FolderBrowserDialogModel; }
        }

        public FolderBrowserDialog()
        {
            InitializeComponent();


            DialogModel.ErrorNotice += onErrorNotice;
        }

        private void onErrorNotice(object i_Sender,NotificationEventArgs<Exception> i_Args)
        {
            System.Diagnostics.Debug.WriteLine(i_Args.Data.Message);

            MessageBox.Show(i_Args.Data.Message + System.Environment.NewLine + Strings.MessegeBoxTextErrorDirectNotFound, Strings.MessegeBoxTitleErrorDirectNotFound, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
