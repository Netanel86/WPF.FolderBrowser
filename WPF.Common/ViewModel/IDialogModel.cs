using System.Windows.Input;

namespace WPF.Common.ViewModel
{
    public interface ILifeCycleHandler : IClosableElement, IErrorNotifier
    {
    }
    public interface IDialogModel<T> : ILifeCycleHandler
    {
        T ReturnValue { get; }
        ICommand CloseCommand { get; }
        ICommand LoadCommand { get; }
        ICommand OkCommand { get; }
        ICommand CancelCommand { get; }
    }
}
