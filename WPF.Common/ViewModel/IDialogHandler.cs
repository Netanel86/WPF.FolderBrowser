using System;
using System.Windows.Input;

namespace WPF.Common.ViewModel
{
    public interface IDialogHandler
    {
        event EventHandler OpenDialogRequest;

        ICommand OpenDialogCommand { get; }
    }
}
