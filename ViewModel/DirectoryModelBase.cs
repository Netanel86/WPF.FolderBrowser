using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FolderBrowserDialog.Localization;

namespace FolderBrowserDialog.ViewModel
{
    public interface ISelectedObserver
    {
        void NotifyIsSelected(DirectoryModelBase i_Directory);
    }
    public class DirectoryModelBase : TreeViewItemModel
    {
        private ISelectedObserver m_SelectedObserver;
        private string m_ImagePath = String.Empty;
        public string ImagePath
        {
            get 
            {
                if (m_ImagePath == String.Empty)
                {
                    this.LoadImage();
                }
                return m_ImagePath; 
            }

            set
            {
                if (m_ImagePath.CompareTo(value) != 0)
                {
                    m_ImagePath = value;
                    this.OnPropertyChanged("ImagePath");
                }
            }
        }
        public string Path
        {
            get { return r_Directory.FullName; }
        }
        protected readonly DirectoryInfo r_Directory;
        public bool IsEmpty
        {
            get { return r_Directory.GetDirectories().Length == 0; }
        }

        private bool m_HasAccess = true;
        public bool HasAccess
        {
            get
            {
                const bool v_Access = true;
                if (m_HasAccess == v_Access)
                {
                    try
                    {
                        m_HasAccess = r_Directory.GetDirectories() != null ? v_Access : !v_Access;
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        m_HasAccess = !v_Access;
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }

                return m_HasAccess;
            }
        }

        public DirectoryModelBase(DirectoryInfo i_Directory, TreeViewItemModel i_ParentDirectory)
            : base(i_ParentDirectory, false)
        {
            CheckIfNull(i_Directory, "i_Directory");
            CheckIfNull(i_ParentDirectory, "i_ParentDirectory");

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
                        FolderModel folder = new FolderModel(subDirectory, this);
                        folder.AddSelectedObserver(m_SelectedObserver); 
                        this.Children.Add(folder);
                    }
                }
            }

        }

        protected virtual void LoadImage()
        {
        }

        protected void CheckIfNull(object i_Parameter, string i_ParameterName)
        {
            if (i_Parameter == null)
            {
                throw new ArgumentNullException(i_ParameterName);
            }
        }

        public void AddSelectedObserver(ISelectedObserver i_Observer)
        {
            this.m_SelectedObserver = i_Observer;
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
                if (this.IsSelected && m_SelectedObserver != null)
                {
                    m_SelectedObserver.NotifyIsSelected(this);
                }
            }
        }
    }
}
