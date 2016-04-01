using System;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace FolderBrowswerDialog.ViewModel
{
    public class DirectoryItemViewModel : TreeItemViewModel
    {
        private static readonly string sr_DriveIconImage = "Icons\\drive-icon.png";
        private static readonly string sr_FolderClosedIconImage = "Icons\\folder-close-blue-icon.png";
        private static readonly string sr_FolderOpenIconImage = "Icons\\folder-open-blue-icon.png";
        private static readonly string sr_MyComputerIconImage = "Icons\\my-computer-icon-2.png";

        private string m_Image = String.Empty;
        public string Image
        {
            get { return m_Image; }

            set
            {
                if (m_Image.CompareTo(value) != 0)
                {
                    m_Image = value;
                    this.OnPropertyChanged("Image");
                }
            }
        }

        private readonly DirectoryTreeViewModel r_Root;

        private readonly DirectoryInfo r_Directory;

        private string m_DirectoryName = String.Empty;
        public string DirectoryName
        {
            get
            {
                return r_Directory != null ? r_Directory.Name : m_DirectoryName;
            }
        }

        public string DirectoryPath
        {
            get { return r_Directory != null ? r_Directory.FullName : this.m_DirectoryName; }
        }

        public bool IsRoot
        {
            get { return r_Directory == null; }
        }

        public bool IsEmpty
        {
            get { return r_Directory.GetDirectories().Length == 0; }
        }

        public bool HasAccess
        {
            get
            {
                const bool v_HasAccess = true;
                bool access = !v_HasAccess;
                try
                {
                    access = r_Directory.GetDirectories() != null ? v_HasAccess : !v_HasAccess;
                }
                catch (UnauthorizedAccessException ex)
                {
                    access = !v_HasAccess;
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                return access;
            }
        }

        public DirectoryItemViewModel(DirectoryTreeViewModel i_Root, DirectoryInfo i_Directory, DirectoryItemViewModel i_ParentDirectory)
            : base(i_ParentDirectory, false)
        {
            checkIfNull(i_Root, "i_Root");
            checkIfNull(i_Directory, "i_Directory");

            r_Root = i_Root;
            r_Directory = i_Directory;

            if (HasAccess && !IsEmpty)
            {
                this.Children.Add(this.DummyItem);
            }

            this.Image = i_ParentDirectory.IsRoot ? sr_DriveIconImage : sr_FolderClosedIconImage;
        }

        private void checkIfNull(object i_Parameter, string i_ParameterName)
        {
            if (i_Parameter == null)
            {
                throw new ArgumentNullException(i_ParameterName);
            }
        }

        /// <summary>
        /// Root dummy <typeparamref name="DirectoryItemViewModel"/> constructor
        /// </summary>
        /// <remarks>
        /// used to create a root object, where both <paramref name="Parent"/>
        /// and Directory members are null
        /// </remarks>
        /// <param name="i_RootDummyName">Name representing the Root object</param>
        public DirectoryItemViewModel(DirectoryTreeViewModel i_Root, string i_RootDummyName)
            : base(null, false)
        {
            checkIfNull(i_Root, "i_Root");

            r_Root = i_Root;
            m_DirectoryName = i_RootDummyName;
            this.Image = sr_MyComputerIconImage;
        }

        protected override void Populate()
        {
            this.Children.Clear();
            if (HasAccess && r_Directory.GetDirectories().Length > 0)
            {
                foreach (DirectoryInfo subDirectory in r_Directory.GetDirectories())
                {
                    if ((subDirectory.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        this.Children.Add(new DirectoryItemViewModel(r_Root, subDirectory, this));
                    }
                }
            }
        }

        public bool MatchDirectoryName(string i_DirectoryToMatch)
        {
            const bool v_Match = true;
            const bool v_IgnoreCase = true;

            //populate the item before searching deeper.
            if (!this.IsPopulated)
            {
                this.Populate();
            }

            return !this.IsRoot && String.Compare(r_Directory.Name, i_DirectoryToMatch, v_IgnoreCase) == 0 ? v_Match : !v_Match;
        }

        protected override void OnPropertyChanged(string i_Property)
        {
            base.OnPropertyChanged(i_Property);

            if (i_Property.CompareTo("IsSelected") == 0)
            {
                r_Root.PathText = this.DirectoryPath;
            }

            if (!this.IsRoot && i_Property.CompareTo("IsExpanded") == 0)
            {
                this.Image = this.IsExpanded ? sr_FolderOpenIconImage : sr_FolderClosedIconImage;
            }
        }

    }
}
