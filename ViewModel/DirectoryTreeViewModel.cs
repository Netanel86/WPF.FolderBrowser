using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace FolderBrowswerDialog.ViewModel
{
    public class DirectoryTreeViewModel : INotifyPropertyChanged
    {
        public DirectoryItemViewModel SelectedItem
        {
            get { return RootItems.FirstOrDefault(i => i.IsSelected); }
        }
        
        private readonly ObservableCollection<DirectoryItemViewModel> r_RootItems;
        
        private DirectoryItemViewModel m_MyComputer;

        public string SearchText 
        {
            get { return m_SearchText; }
            set 
            { 
                m_SearchText = value;
                m_SearchCommand.CanExecute(value);
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("SearchText"));
            }
        }
        private string m_SearchText = String.Empty;

        public ObservableCollection<DirectoryItemViewModel> RootItems
        {
            get { return r_RootItems; }
        }
        
        public IEnumerable<DirectoryItemViewModel> m_MatchingPaths;
        
        public DirectoryTreeViewModel(DriveInfo[] i_SystemDrives)
        {
            r_RootItems = new ObservableCollection<DirectoryItemViewModel>();
            m_MyComputer = new DirectoryItemViewModel(this, "My Computer");
            m_SearchCommand = new RelayCommand(SearchForDirectory, canexecute);
            
            foreach (DriveInfo drive in i_SystemDrives)
            {
                if (drive.DriveType != DriveType.CDRom)
                {
                    m_MyComputer.Children.Add(new DirectoryItemViewModel(this, drive.RootDirectory, null));
                }
            }

            r_RootItems.Add(m_MyComputer);
        }
        
        public IEnumerable<DirectoryItemViewModel> SearchForPath(string i_PathSearchString, DirectoryItemViewModel folder)
        {
            if (folder.MatchPath(i_PathSearchString))
            {
                yield return folder;
            }
            
            foreach (DirectoryItemViewModel sub in folder.Children)
            {
                foreach (DirectoryItemViewModel match in SearchForPath(i_PathSearchString, sub))
                {
                    yield return match;
                }
            }

        }

        public void SearchForDirectory(object obj)
        {

        }
        private bool canexecute(object obj)
        {
            return this.SearchText != String.Empty;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(sender, args);
            }
        }

        public ICommand SearchCommand 
        { 
            get { return m_SearchCommand; } 
        }
        private readonly ICommand m_SearchCommand;
    }

    public class RelayCommand : ICommand
    {
        private Predicate<object> m_CanExecute;
        private Action<object> m_Execute;
        
        private bool m_CanExecuteCurrentState;

        public RelayCommand(Action<object> i_Execute, Predicate<object> i_CanExecute)
        {
            if(i_Execute == null )
            {
                throw new ArgumentNullException("i_Execute");
            }

            if(i_CanExecute == null )
            {
                throw new ArgumentNullException("i_CanExecute");
            }
            
            m_CanExecute = i_CanExecute;
            m_Execute = i_Execute;
        }

        public RelayCommand(Action<object> i_Execute)
            : this(i_Execute, x => true)
        { }

        public bool CanExecute(object parameter)
        {
            if (this.m_CanExecute != null)
            {
                bool canExecute = this.m_CanExecute(parameter);
                if (m_CanExecuteCurrentState != canExecute)
                {
                    m_CanExecuteCurrentState = canExecute;
                    OnCanExecuteChanged();
                }
            }

            return m_CanExecuteCurrentState;
        }

        public event EventHandler CanExecuteChanged;

        protected void OnCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public void Execute(object parameter)
        {
            this.m_Execute(parameter);
        }
    }
}
