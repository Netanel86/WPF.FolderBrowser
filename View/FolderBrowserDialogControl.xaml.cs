﻿using System.IO;
using System.Windows.Controls;
using FolderBrowserDialog.ViewModel;

namespace FolderBrowserDialog.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FolderBrowserDialogControl : UserControl
    {
        public DirectoryTreeViewModel viewModel;
        public FolderBrowserDialogControl()
        {
            InitializeComponent();
            viewModel = new DirectoryTreeViewModel(DriveInfo.GetDrives());
            base.DataContext = viewModel;
        }
    }
}
