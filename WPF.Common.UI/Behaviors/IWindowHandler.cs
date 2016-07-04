using System;

namespace WPF.Common.UI.Behaviors
{
    public interface IWindowHandler
    {
        event EventHandler CloseWindowRequest;
    }
}
