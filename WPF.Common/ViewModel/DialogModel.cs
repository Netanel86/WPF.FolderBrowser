﻿using System;
using System.Windows.Input;

namespace WPF.Common.ViewModel
{
    public abstract class DialogModel<T> : ViewModelBase, IDialogModel<T>
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

        public event EventHandler CloseRequest;

        public event EventHandler ErrorNotice;

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
            OkCommand = new RelayCommand((x) => CloseWindow(!v_DialogCanceled), (x) => CheckResultLegitimacy());
            CancelCommand = new RelayCommand((x) => CloseWindow(v_DialogCanceled));
            CloseCommand = new RelayCommand((x) => OnClosed());
            LoadCommand = new RelayCommand((x) => OnLoaded());
        }

        protected abstract bool CheckResultLegitimacy();

        protected virtual void OnErrorNotice(ErrorMessage i_Message)
        {
            if (this.ErrorNotice != null)
            {
                ErrorNotice(this, new NotificationEventArgs<ErrorMessage>(i_Message));
            }
        }

        protected virtual void CloseWindow(bool i_DialogCanceled)
        {
            if (this.CloseRequest != null)
            {
                this.CloseRequest(this, new NotificationEventArgs<bool>(i_DialogCanceled));
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
