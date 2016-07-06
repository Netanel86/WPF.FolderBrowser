using System.Windows.Input;

namespace WPF.Common.ViewModel
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
