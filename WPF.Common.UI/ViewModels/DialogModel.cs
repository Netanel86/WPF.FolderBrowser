using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WPF.Common.UI.ViewModels
{
    public abstract class DialogModel<T> : BindableObject, IDialogModel<T>
    {
        private T m_ReturnValue;
        
        public T ReturnValue
        {
            get { return m_ReturnValue; }
            set
            {
                m_ReturnValue = value;
                this.OnPropertyChanged("ReturnValue");
            }
        }

        private const bool v_DialogCanceled = true;

        public event EventHandler CloseWindowRequest;

        public bool IsLegitReturnValue
        {
            get { return CheckResultLegitimacy(); }
        }

        public ICommand OkCommand
        { get; private set; }

        public ICommand CancelCommand
        { get; private set; }

        public ICommand CloseCommand
        { get; private set; }

        public ICommand LoadCommand
        { get; private set; }

        public DialogModel()
        {
            OkCommand = new RelayCommand((x) => CloseWindow(!v_DialogCanceled), (x) => IsLegitReturnValue);
            CancelCommand = new RelayCommand((x) => CloseWindow(v_DialogCanceled));
            CloseCommand = new RelayCommand((x) => OnClosed());
            LoadCommand = new RelayCommand((x) => OnLoaded());
        }

        protected virtual bool CheckResultLegitimacy()
        {
            throw new NotImplementedException();
        }

        protected virtual void CloseWindow(bool i_DialogCanceled)
        {
            if (this.CloseWindowRequest != null)
            {
                this.CloseWindowRequest(this, new NotificationEventArgs<bool>(i_DialogCanceled));
            }
        }

        protected virtual void OnClosed()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnLoaded()
        {
            throw new NotImplementedException();
        }
    }
}
