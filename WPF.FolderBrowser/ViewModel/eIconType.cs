using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Collections;
using System.Windows;

namespace WPF.FolderBrowserDialog.ViewModel
{
    /// <summary>
    /// An Enumeration represnting the types of available icons. 
    /// </summary>
    /// <remarks>
    /// Used as a representaion of icons in ViewModel's
    /// </remarks>
    public enum eIconType
    {
        None,
        Find,
        Computer,
        SimpleDrive,
        NetworkDrive,
        RemovableDrive,
        SystemDrive,
        ClosedFolder,
        OpenFolder,
        NoAccessFolder,
        NewFolder,
        RenameFolder,
        ExpanderClosed,
        ExpanderOpen
    }
}
