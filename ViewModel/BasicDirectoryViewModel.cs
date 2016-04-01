using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FolderBrowserDialog.ViewModel
{
    public class BasicDirectoryViewModel : TreeItemViewModel
    {
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
        public string Path
        {
            get { return r_Directory.FullName; }
        }
        protected readonly DirectoryInfo r_Directory;
        protected readonly DirectoryTreeViewModel r_Root;
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

        public BasicDirectoryViewModel(DirectoryTreeViewModel i_Root, DirectoryInfo i_Directory, TreeItemViewModel i_ParentDirectory)
            : base(i_ParentDirectory, false)
        {
            CheckIfNull(i_Root, "i_Root");
            CheckIfNull(i_Directory, "i_Directory");
            CheckIfNull(i_ParentDirectory, "i_ParentDirectory");

            r_Root = i_Root;
            r_Directory = i_Directory;

            if (HasAccess && !IsEmpty)
            {
                this.Children.Add(this.DummyItem);
            }
        }

        protected override void Populate()
        {
            this.Children.Clear();
            if (this.HasAccess)
            {
                foreach (DirectoryInfo subDirectory in r_Directory.GetDirectories())
                {
                    if ((subDirectory.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        this.Children.Add(new FolderViewModel(r_Root, subDirectory, this));
                    }
                }
            }

        }

        protected void CheckIfNull(object i_Parameter, string i_ParameterName)
        {
            if (i_Parameter == null)
            {
                throw new ArgumentNullException(i_ParameterName);
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

            return String.Compare(r_Directory.Name, i_DirectoryToMatch, v_IgnoreCase) == 0 ? v_Match : !v_Match;
        }

        protected override void OnPropertyChanged(string i_Property)
        {
            base.OnPropertyChanged(i_Property);

            if (i_Property.CompareTo("IsSelected") == 0)
            {
                r_Root.PathText = this.Path;
            }
        }
    }
}
