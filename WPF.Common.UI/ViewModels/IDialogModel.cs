using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPF.Common.UI.Behaviors;
using System.Windows.Input;

namespace WPF.Common.UI.ViewModels
{
    public interface IDialogModel<T> : IWindowHandler
    {
        T ReturnValue { get; }
        ICommand CloseCommand { get; }
        ICommand LoadCommand { get; }
        ICommand OkCommand { get; }
        ICommand CancelCommand { get; }
    }
}
