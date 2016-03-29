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

        public string FullPath
        {
            get { return r_Directory != null ? r_Directory.FullName : this.m_DirectoryName ; }
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

        public DirectoryItemViewModel(DirectoryTreeViewModel i_Root,DirectoryInfo i_Directory, DirectoryItemViewModel i_ParentDirectory)
            :base(i_ParentDirectory, false)
        {
            r_Root = i_Root;
            r_Directory = i_Directory;
            if (HasAccess && !IsEmpty)
            {
                this.Children.Add(this.DummyItem);
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
            r_Root = i_Root;
            m_DirectoryName = i_RootDummyName;
        }
        
        protected override void Populate()
        {
            this.Children.Clear();
            if (HasAccess)
            {
                foreach (DirectoryInfo subDirectory in r_Directory.GetDirectories())
                {
                    if ((subDirectory.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        this.Children.Add(new DirectoryItemViewModel(r_Root,subDirectory,this));
                    }
                }
            }
        }

        public bool MatchPath(string i_PathSearchString)
        {
            const bool v_Match = true;
            return r_Directory.FullName.CompareTo(i_PathSearchString) > -1 ? v_Match : !v_Match;
        }
        
        protected override void OnPropertyChanged(string i_Property)
        {
            base.OnPropertyChanged(i_Property);

            if (i_Property.CompareTo("IsSelected") == 0)
            {
               r_Root.SearchText = this.FullPath;
            }
        }

    }
}
