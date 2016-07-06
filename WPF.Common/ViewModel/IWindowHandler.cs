using System;

namespace WPF.Common.ViewModel
{
    public interface IWindowHandler
    {
        event EventHandler CloseWindowRequest;
    }
}
