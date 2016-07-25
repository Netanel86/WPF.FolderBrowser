using System;
using System.Windows.Input;

namespace WPF.Common.ViewModel
{
    public interface IDialogPresenter
    {
        event EventHandler OpenDialogRequest;

        ICommand OpenDialogCommand { get; }
    }
}
